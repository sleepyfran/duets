[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.RoomScene

open Duets.Cli.Components
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Components.Scene.Room
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Data
open Duets.Data.Scenes
open Duets.Entities
open Duets.Simulation
open FSharp.Data.UnitSystems.SI.UnitNames
open Spectre.Console

let private template state =
    let cityId, _, _ = Queries.World.currentCoordinates state
    let place = Queries.World.currentPlace state
    let room = Queries.World.currentRoom state

    Scenes.resolve (sceneWidth AnsiConsole.Profile.Width) cityId place room.RoomType

/// Returns whether the current room has a template to show.
let canShow state =
    let room = Queries.World.currentRoom state

    match room.RoomType, Queries.Situations.current state with
    | _, SituationTypes.Travelling _ -> false
    | RoomType.Street, _ -> false
    | _ -> template state |> Option.isSome

let private exitsOf (interactions: InteractionWithMetadata list) =
    interactions
    |> List.choose (fun interaction ->
        match interaction.Interaction with
        | Interaction.FreeRoam(FreeRoamInteraction.Move(direction, roomId)) ->
            Some(ExitTarget.Room direction, Choice1Of2 roomId)
        | Interaction.FreeRoam(FreeRoamInteraction.GoOut streetId) ->
            Some(ExitTarget.Street, Choice2Of2 streetId)
        | _ -> None)

let private showOf (place: Place) ongoingConcert =
    match place.PlaceType with
    | ConcertSpace venue -> Show.fromConcert venue.Capacity ongoingConcert |> Some
    | _ -> None

let private concertState state (place: Place) =
    let bandMembers () =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter state
        |> List.length

    match Queries.Situations.current state with
    | SituationTypes.Concert(SituationTypes.InConcert ongoingConcert) ->
        bandMembers (), showOf place ongoingConcert
    | SituationTypes.Concert _ -> bandMembers (), None
    | _ -> 0, None

let private context state exits : RoomRenderer.RoomContext =
    let coords = Queries.World.currentCoordinates state
    let cityId, _, roomId = coords
    let place = Queries.World.currentPlace state
    let room = Queries.World.currentRoom state
    let knownPeople, unknownPeople = Queries.World.peopleInCurrentPlace state
    let bandMembers, show = concertState state place

    { Skin = CitySkin.forCity cityId
      Lighting =
        { DayMoment = Queries.Calendar.today state |> Calendar.Query.dayMomentOf
          Weather = Queries.World.currentWeather state }
      Quality = place.Quality
      Seed = seedOf $"{place.Id}{roomId}"
      KnownPeople = knownPeople.Length
      UnknownPeople = unknownPeople.Length
      BandMembers = bandMembers
      Items = Queries.Items.allIn state coords
      Exits = exits |> List.map fst
      Show = show
      MerchStand = Queries.Merch.standQueue state cityId place.Id
      Cuisine =
        match room.RoomType with
        | RoomType.Restaurant cuisine -> Some cuisine
        | _ -> None }

let private showExits state exits =
    let cityId, placeId, _ = Queries.World.currentCoordinates state

    exits
    |> List.map (fun (target, destination) ->
        let arrow =
            match target with
            | ExitTarget.Room direction -> SceneStyles.directionArrow direction
            | ExitTarget.Street -> string SceneStyles.Room.streetExit

        let name =
            match destination with
            | Choice1Of2 roomId ->
                let room = Queries.World.roomById cityId placeId roomId
                World.roomName room.RoomType |> Styles.room
            | Choice2Of2 streetId ->
                let street = Queries.World.streetInCurrentCity streetId state
                street.Name |> Styles.place

        $"{Styles.direction arrow} {name}")
    |> function
        | [] -> ()
        | exits -> World.roomSceneExits (Generic.listOf exits id) |> showMessage

/// Returns the padding that centers the template, if it fits.
let private paddingFor (template: Template) =
    let width = sceneWidth AnsiConsole.Profile.Width

    if width >= compactWidth && width >= template.Width then
        String.replicate (max 0 ((width - template.Width) / 2)) " " |> Some
    else
        None

/// Renders the current room with its people, items and exits.
let show state (interactions: InteractionWithMetadata list) =
    let place = Queries.World.currentPlace state
    let room = Queries.World.currentRoom state
    let exits = exitsOf interactions
    let context = context state exits

    World.roomSceneHeader room.RoomType place.Name context.Lighting.DayMoment
    |> showMessage

    template state
    |> Option.iter (fun template ->
        paddingFor template
        |> Option.iter (fun padding ->
            RoomRenderer.build context template
            |> SceneGrid.toMarkup
            |> List.iter (fun line -> padding + line |> showMessage))

        Caption.create context template room.RoomType
        |> Styles.faded
        |> showMessage)

    showExits state exits

let private framesPerSecond = 4
let private framesPerReaction = 8

let private songProgress title width frame frames =
    let played = width * (frame + 1) / frames

    let bar =
        String.replicate played (string SceneStyles.Room.songProgressDone)
        + String.replicate (width - played) (string SceneStyles.Room.songProgressLeft)

    $"{title} [{SceneStyles.Room.songProgressColor}]{bar}[/]"

/// Returns whether the stage of the current room can be animated.
let canAnimate state =
    canShow state && template state |> Option.bind paddingFor |> Option.isSome

/// Animates the stage while a song plays.
let animateSong state ongoingConcert energy title (duration: int<second>) =
    let place = Queries.World.currentPlace state
    let exits = Queries.Interactions.availableCurrently state |> exitsOf

    match template state, showOf place ongoingConcert with
    | Some template, Some show ->
        let context =
            { context state exits with
                Show = Some { show with Energy = Some energy } }
        let padding = paddingFor template |> Option.defaultValue ""
        let reactions = Caption.songReactions show
        let frames = max 1 (int duration * framesPerSecond)

        let reactionAt frame =
            let random = System.Random(context.Seed + frame / framesPerReaction)
            reactions |> List.item (random.Next(reactions.Length))

        let render frame =
            [ yield songProgress title template.Width frame frames
              yield!
                  RoomRenderer.buildFrame frame context template
                  |> SceneGrid.toMarkup
                  |> List.map ((+) padding)
              yield reactionAt frame |> Styles.faded ]
            |> List.map (fun line -> Markup(line) :> Rendering.IRenderable)
            |> Rows

        AnsiConsole
            .Live(render 0)
            .AutoClear(true)
            .Start(fun ctx ->
                for frame in 1 .. frames - 1 do
                    System.Threading.Thread.Sleep(1000 / framesPerSecond)
                    ctx.UpdateTarget(render frame))

        System.Threading.Thread.Sleep(1000 / framesPerSecond)
    | _ -> ()
