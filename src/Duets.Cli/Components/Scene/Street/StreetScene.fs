[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.StreetScene

open Duets.Cli.Components
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Components.Scene.Street
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Data.Scenes
open Duets.Entities
open Duets.Simulation
open Spectre.Console

let private gap = 2
let private skyHeight = 2

let private landmarkStyle landmark fallback glyph =
    Landmark.styleOf landmark glyph |> Option.defaultValue fallback

let private landmarkProps (landmark: Landmark) : CitySkin.StreetProp list =
    landmark.Props
    |> List.map (fun prop ->
        prop
        |> Seq.rev
        |> Seq.map (fun glyph ->
            glyph, landmarkStyle landmark SceneStyles.Colors.grey62 glyph)
        |> List.ofSeq)

/// Builds the grid for one row of façades. The first row includes the sky
/// above the buildings and the last one the road.
let buildRow
    (skin: CitySkin.Skin)
    (lighting: Lighting)
    width
    (places: (Place * bool * Highlights.Highlight list) list)
    isFirst
    isLast
    people
    (skyline: Template option)
    (landmarks: Landmark list)
    seed
    =
    let buildingsHeight =
        places
        |> List.map (fun (place, _, _) -> FacadeRenderer.heightOf skin place) |> List.fold max 1

    let artOf layer =
        landmarks
        |> List.filter (fun landmark -> landmark.Layer = layer)
        |> List.choose _.Art

    let skyline = if isFirst then skyline else None
    let backArt = if isFirst then artOf LandmarkLayer.Back else []
    let frontArt = artOf LandmarkLayer.Front
    let sky = if isFirst then skyHeight else 0

    let road =
        landmarks |> List.filter (fun landmark -> not landmark.Road.IsEmpty) |> List.tryLast

    let roadHeight =
        match road with
        | Some landmark when isLast -> landmark.Road.Length
        | None when isLast -> 1
        | _ -> 0

    let backdropHeight =
        Option.toList skyline @ backArt |> List.map _.Height |> List.fold max 0

    (* Leave a row on top of the skyline for the sun and the moon. *)
    let groundRow = max (sky + buildingsHeight) (backdropHeight + 1)

    let height = groundRow + 1 + roadHeight

    let grid = SceneGrid.create width height

    if isFirst then
        Sky.draw grid lighting

    let rowWidth =
        places.Length * Facades.width + (places.Length - 1) * gap

    let startX = max 0 ((width - rowWidth) / 2)

    (* The skyline only shows above the roofs, not in the gaps. *)
    let behindBuildings x y =
        places
        |> List.indexed
        |> List.exists (fun (index, (place, _, _)) ->
            let left = startX + index * (Facades.width + gap) - 1
            let right = left + Facades.width + 1
            let top = groundRow - FacadeRenderer.heightOf skin place

            x >= left && x <= right && y >= top)

    Option.toList skyline @ backArt
    |> List.iter (fun art ->
        Skyline.draw grid lighting art (groundRow - 1) behindBuildings seed)

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

    let gaps =
        [ yield startX - gap
          for index in 1 .. places.Length - 1 do
              yield startX + index * (Facades.width + gap) - gap
          yield startX + rowWidth + gap - 1 ]
        |> List.filter (fun x -> x >= 0 && x < width)

    let propsSkin =
        landmarks
        |> List.filter (fun landmark -> not landmark.Props.IsEmpty)
        |> List.tryLast
        |> Option.map (fun landmark ->
            { skin with
                StreetProps = landmarkProps landmark })
        |> Option.defaultValue skin

    Furniture.drawProps grid propsSkin lighting gaps groundRow seed
    Sky.drawParticles grid lighting groundRow seed

    Ground.drawSidewalk grid skin groundRow

    landmarks
    |> List.iter (fun landmark ->
        landmark.Sidewalk
        |> Option.iter (fun pattern ->
            Ground.drawPattern
                grid
                groundRow
                pattern
                (landmarkStyle landmark skin.SidewalkColor)))

    if isLast then
        match road with
        | Some landmark ->
            landmark.Road
            |> List.iteri (fun index pattern ->
                Ground.drawPattern
                    grid
                    (groundRow + 1 + index)
                    pattern
                    (landmarkStyle landmark skin.RoadColor))
        | None -> Ground.drawRoad grid skin (groundRow + 1)

        if landmarks |> List.forall _.Traffic then
            Furniture.drawTraffic grid skin lighting (groundRow + 1) seed

        frontArt
        |> List.iter (fun art ->
            Skyline.draw grid lighting art groundRow (fun _ _ -> false) seed)

        Ground.drawPeople grid groundRow people seed

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
    let skyline = resolveSkyline cityId zone
    let landmarks = resolveLandmarks street

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
                skyline
                landmarks
                (seed + index)
            |> SceneGrid.toMarkup
            |> List.iter showMessage)

    let peopleCount = (fst people).Length + (snd people).Length

    places
    |> List.map (fun (place, isOpen, _) -> place, isOpen)
    |> Caption.create
        cityId
        street
        (landmarks |> List.collect _.Captions)
        roomId
        lighting
        peopleCount
    |> Styles.faded
    |> showMessage

    Legend.showPlaces places
    Legend.showDirections state place street interactions
