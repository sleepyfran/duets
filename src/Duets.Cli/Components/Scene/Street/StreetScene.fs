[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.StreetScene

open Duets.Cli.Components
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Components.Scene.Street
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation
open Spectre.Console

let private gap = 2
let private skyHeight = 2

/// Builds the grid for one row of façades. The first row includes the sky
/// above the buildings and the last one the road.
let private buildRow
    skin
    (lighting: Lighting)
    width
    (places: (Place * bool * Highlights.Highlight list) list)
    isFirst
    isLast
    people
    seed
    =
    let buildingsHeight =
        places
        |> List.map (fun (place, _, _) -> FacadeRenderer.heightOf place) |> List.fold max 1

    let sky = if isFirst then skyHeight else 0
    let groundRow = sky + buildingsHeight
    let height = groundRow + 1 + (if isLast then 1 else 0)

    let grid = SceneGrid.create width height

    if isFirst then
        Sky.draw grid lighting

    let rowWidth =
        places.Length * Facades.width + (places.Length - 1) * gap

    let startX = max 0 ((width - rowWidth) / 2)

    places
    |> List.iteri (fun index (place, isOpen, highlights) ->
        let x = startX + index * (Facades.width + gap)

        FacadeRenderer.draw
            grid
            skin
            lighting
            x
            (groundRow - 1)
            place
            isOpen
            highlights)

    Sky.drawParticles grid lighting groundRow seed

    if isLast then
        Ground.drawSidewalk grid skin groundRow (Some people) seed
        Ground.drawRoad grid skin (groundRow + 1)
    else
        Ground.drawSidewalk grid skin groundRow None seed

    grid

/// Returns whether the player is currently walking down a street, which is
/// when the street scene can be shown.
let canShow state =
    let currentRoom = Queries.World.currentRoom state

    match currentRoom.RoomType, Queries.Situations.current state with
    | _, SituationTypes.Travelling _ -> false
    | RoomType.Street, _ -> true
    | _ -> false

/// Renders the current street, including all the places in the part of the
/// street where the player is, the weather and the time of the day, followed
/// by a legend with the places and where the player can go from here.
let show state (interactions: InteractionWithMetadata list) =
    let cityId, _, roomId = Queries.World.currentCoordinates state
    let place = Queries.World.currentPlace state
    let street = Queries.World.streetInCurrentCity place.Id state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId
    let skin = CitySkin.forCity cityId

    let lighting: Lighting =
        { DayMoment = Queries.Calendar.today state |> Calendar.Query.dayMomentOf
          Weather = Queries.World.currentWeather state }

    let places =
        interactions
        |> List.tryPick (fun interaction ->
            match interaction.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Enter places) ->
                Some places
            | _ -> None)
        |> Option.defaultValue []
        |> List.map (fun place ->
            place,
            Queries.World.placeCurrentlyOpen' state place,
            Highlights.forPlace state cityId place)

    let part =
        match street.Type with
        | StreetType.Split(_, splits) -> Some(int roomId + 1, splits)
        | StreetType.OneWay -> None

    World.streetSceneHeader street.Name zone.Name part lighting.DayMoment
    |> showMessage

    let width = sceneWidth AnsiConsole.Profile.Width
    let perRow = max 1 ((width + gap) / (Facades.width + gap))

    let rows =
        match places with
        | [] -> [ [] ]
        | places -> places |> List.chunkBySize perRow

    let people = Queries.World.peopleInCurrentPlace state
    let seed = seedOf $"{place.Id}{roomId}"

    (* On very narrow terminals the façades take a row each and the sky
       barely fits, so we skip the art and rely on the caption and legend. *)
    if width >= compactWidth then
        rows
        |> List.iteri (fun index row ->
            buildRow
                skin
                lighting
                width
                row
                (index = 0)
                (index = rows.Length - 1)
                people
                (seed + index)
            |> SceneGrid.toMarkup
            |> List.iter showMessage)

    let peopleCount = (fst people).Length + (snd people).Length

    places
    |> List.map (fun (place, isOpen, _) -> place, isOpen)
    |> Caption.create street roomId lighting peopleCount
    |> Styles.faded
    |> showMessage

    Legend.showPlaces places
    Legend.showDirections state place street interactions
