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
open Spectre.Console

let private template state =
    let cityId, _, _ = Queries.World.currentCoordinates state
    let place = Queries.World.currentPlace state
    let room = Queries.World.currentRoom state

    Scenes.resolve cityId place room.RoomType

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

let private concertState state (place: Place) =
    let bandMembers () =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter state
        |> List.length

    match Queries.Situations.current state, place.PlaceType with
    | SituationTypes.Concert(SituationTypes.InConcert ongoingConcert), ConcertSpace venue ->
        let crowd =
            float ongoingConcert.Concert.TicketsSold / float (max 1 venue.Capacity)

        bandMembers (), min 1.0 crowd, true
    | SituationTypes.Concert _, _ -> bandMembers (), 0.0, false
    | _ -> 0, 0.0, false

let private context state exits : RoomRenderer.RoomContext =
    let coords = Queries.World.currentCoordinates state
    let cityId, _, roomId = coords
    let place = Queries.World.currentPlace state
    let room = Queries.World.currentRoom state
    let knownPeople, unknownPeople = Queries.World.peopleInCurrentPlace state
    let bandMembers, crowd, concertOngoing = concertState state place

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
      Crowd = crowd
      ConcertOngoing = concertOngoing
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
        let width = sceneWidth AnsiConsole.Profile.Width

        if width >= compactWidth && width >= template.Width then
            let padding =
                String.replicate (max 0 ((width - template.Width) / 2)) " "

            RoomRenderer.build context template
            |> SceneGrid.toMarkup
            |> List.iter (fun line -> padding + line |> showMessage)

        Caption.create context template room.RoomType
        |> Styles.faded
        |> showMessage)

    showExits state exits
