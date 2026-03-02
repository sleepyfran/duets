module Duets.UI.Common.Text.World.Places

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let roomName roomType =
    match roomType with
    | RoomType.Backstage -> "backstage"
    | RoomType.Bar -> "bar area"
    | RoomType.Bedroom -> "bedroom"
    | RoomType.BoardingGate -> "boarding gate"
    | RoomType.Cafe -> "cafe area"
    | RoomType.CasinoFloor -> "casino floor"
    | RoomType.ChangingRoom -> "changing room"
    | RoomType.Gym -> "gym"
    | RoomType.Kitchen -> "kitchen"
    | RoomType.LivingRoom -> "living room"
    | RoomType.Lobby -> "lobby"
    | RoomType.MasteringRoom -> "mastering room"
    | RoomType.Platform -> "platform"
    | RoomType.ReadingRoom -> "reading room"
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant _ -> "restaurant"
    | RoomType.ScreeningRoom -> "screening room"
    | RoomType.SecurityControl -> "security control"
    | RoomType.ShowRoom -> "show room"
    | RoomType.Stage -> "stage"
    | RoomType.Street -> "street"
    | RoomType.Workshop -> "workshop"

let private directionName =
    function
    | North -> "north"
    | NorthEast -> "north-east"
    | East -> "east"
    | SouthEast -> "south-east"
    | South -> "south"
    | SouthWest -> "south-west"
    | West -> "west"
    | NorthWest -> "north-west"

let roomConnections state (interactions: InteractionWithMetadata list) =
    let cityId, placeId, _ = Queries.World.currentCoordinates state
    let place = Queries.World.placeInCityById cityId placeId

    let connections =
        interactions
        |> List.choose (fun i ->
            match i.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Move(direction, roomId)) ->
                Some(direction, Queries.World.roomById cityId placeId roomId)
            | _ -> None)

    let connectedStreetsOpt =
        interactions
        |> List.choose (fun i ->
            match i.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.GoToStreet streets) -> Some streets
            | _ -> None)
        |> List.concat
        |> function
            | [] -> None
            | streets -> streets |> List.map (fun s -> s.Name) |> String.concat ", " |> Some

    match connections, place.PlaceType with
    | [], PlaceType.Street ->
        connectedStreetsOpt
        |> Option.map (fun streets -> $"This street connects to {streets}.")
        |> Option.defaultValue ""
    | [], _ -> "There are no more rooms connecting to this one."
    | connections, PlaceType.Street ->
        let dirs =
            connections |> List.map (fun (dir, _) -> directionName dir) |> String.concat ", "

        let extra =
            connectedStreetsOpt
            |> Option.map (fun s -> $" It also connects to {s}.")
            |> Option.defaultValue "."

        $"The street continues to the {dirs}{extra}"
    | connections, _ ->
        let desc =
            connections
            |> List.map (fun (dir, room) -> $"a {roomName room.RoomType} to the {directionName dir}")
            |> String.concat ", "

        $"There is {desc}."

let roomEntrances (interactions: InteractionWithMetadata list) : string option =
    let entrances =
        interactions
        |> List.choose (fun i ->
            match i.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Enter places) -> Some places
            | _ -> None)
        |> List.concat

    match entrances with
    | [] -> None
    | entrances ->
        let names = entrances |> List.map (fun p -> p.Name) |> String.concat ", "
        let verb = if entrances.Length = 1 then "is an entrance" else "are entrances"
        Some $"There {verb} towards {names}."

let roomExits state (interactions: InteractionWithMetadata list) : string option =
    let exits =
        interactions
        |> List.choose (fun i ->
            match i.Interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.GoOut streetId) -> Some streetId
            | _ -> None)

    match exits with
    | [] -> None
    | exits ->
        let names =
            exits
            |> List.map (fun streetId -> (Queries.World.streetInCurrentCity streetId state).Name)
            |> String.concat ", "

        Some $"There is an exit towards {names} leading out of this place."

let youAreIn state (place: Place) roomType =
    let situation = Queries.Situations.current state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    match roomType, situation with
    | _, Travelling(TravellingByCar _) -> "You are currently in your car."
    | RoomType.Platform, Travelling TravellingByMetro ->
        "You are currently travelling on the metro."
    | RoomType.Street, _ ->
        $"You stand outside on {place.Name}, {zone.Name}"
    | _ ->
        $"You are in the {roomName roomType} at {place.Name}"
