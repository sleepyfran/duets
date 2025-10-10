[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.World

open Duets.Agents
open Duets.Cli.Text
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Entities.World.Place.Type
open Duets.Simulation

let placeNameWithOpeningInfo placeDetails currentlyOpen =
    match currentlyOpen with
    | true -> placeDetails
    | false ->
        let closedText = Styles.faded "(Closed)"
        $"{placeDetails} {closedText}"

let placeNameWithOpeningInfo' (place: Place) currentlyOpen =
    placeNameWithOpeningInfo place.Name currentlyOpen

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.Bookstore -> "Bookstore"
    | PlaceTypeIndex.Cafe -> "Café"
    | PlaceTypeIndex.CarDealer -> "Car dealer"
    | PlaceTypeIndex.Casino -> "Casino"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Gym -> "Gym"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.Hotel -> "Hotel"
    | PlaceTypeIndex.MetroStation -> "Metro station"
    | PlaceTypeIndex.MerchandiseWorkshop -> "Merchandise workshop"
    | PlaceTypeIndex.RadioStudio -> "Radio Studio"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Restaurant -> "Restaurant"
    | PlaceTypeIndex.Studio -> "Studio"
    | PlaceTypeIndex.Street -> "Street"

let placeTypeName' (place: Place) =
    place.PlaceType |> toIndex |> placeTypeName

let roomName (room: RoomType) =
    match room with
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
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.ReadingRoom -> "reading room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant _ -> "restaurant"
    | RoomType.SecurityControl -> "security control"
    | RoomType.ShowRoom -> "show room"
    | RoomType.Stage -> "stage"
    | RoomType.Street -> "street"
    | RoomType.Workshop -> "workshop"

let directionName direction =
    match direction with
    | North -> "north"
    | NorthEast -> "north-east"
    | East -> "east"
    | SouthEast -> "south-east"
    | South -> "south"
    | SouthWest -> "south-west"
    | West -> "west"
    | NorthWest -> "north-west"

let youAreInMessage (place: Place) roomType =
    let state = State.get ()
    let situation = Queries.Situations.current state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    match roomType, situation with
    | _, Travelling(TravellingByCar _) -> "You are currently in your car."
    | RoomType.Platform, Travelling TravellingByMetro ->
        "You are currently travelling on the metro."
    | RoomType.Street, _ ->
        $"You stand outside on {place.Name |> Styles.room}, {zone.Name |> Styles.place}"
    | _ ->
        $"You are in the {roomName roomType |> Styles.room} inside of {place.Name |> Styles.place}"

let noConnectionsToRoom place connectedStreetsMessageOpt =
    match place.PlaceType with
    | PlaceType.Street ->
        connectedStreetsMessageOpt
        |> Option.map (fun connectedStreets ->
            $"This street connects to {connectedStreets}.")
        |> Option.defaultValue ""
    | _ -> "There are no more rooms connecting to this one."

let connectingNodes
    place
    (connections: (Direction * Room) list)
    connectedStreetsMessageOpt
    =
    match place.PlaceType with
    | PlaceType.Street ->
        let connections =
            Generic.listOf connections (fun (direction, _) ->
                directionName direction |> Styles.direction)

        let connectedStreetsMessage =
            connectedStreetsMessageOpt
            |> Option.map (fun connectedStreets ->
                $"It also connects to {connectedStreets}")
            |> Option.defaultValue ""

        $"The street continues to the {connections}. {connectedStreetsMessage}."
    | _ ->
        let connectionsDescription =
            Generic.listOf connections (fun (direction, room) ->
                let roomName = roomName room.RoomType

                $"{Generic.indeterminateArticleFor roomName} {roomName |> Styles.room} to the {directionName direction |> Styles.direction}")

        $"There is {connectionsDescription}."

let private arrivalMessage roomType =
    let characterIsUninspired =
        (State.get (), MoodletType.NotInspired)
        ||> Queries.Characters.playableCharacterHasMoodlet

    match roomType with
    | RoomType.RehearsalRoom when characterIsUninspired ->
        "You're currently feeling uninspired, composing or improving songs will not be easy"
        |> Styles.warning
        |> Some
    | _ -> None

let placeArrivalMessage place roomType =
    match place.PlaceType with
    | PlaceType.RehearsalSpace _ -> arrivalMessage roomType
    | _ -> None

let placeClosedError (place: Place) =
    $"{place.Name} is currently closed. Try again during their opening hours"
    |> Styles.error

let placeNameWithType (place: Place) =
    match place.PlaceType with
    | Home -> place.Name
    | _ -> $"""{place.Name} {$"({placeTypeName' place})" |> Styles.faded}"""
    |> Styles.place

let placeOpeningHours place =
    match place.OpeningHours with
    | PlaceOpeningHours.OpeningHours(daysOfWeek, dayMoments) ->
        let openingDays =
            match daysOfWeek with
            | days when days = Calendar.everyDay -> "Everyday"
            | days when days = Calendar.weekday -> "Monday to Friday"
            | _ -> Generic.listOf daysOfWeek Generic.dayName

        let openingHours = Generic.listOf dayMoments Generic.dayMomentName

        $"{Styles.place place.Name} opens {Styles.time openingDays} @ {openingHours}"
    | _ ->
        (* Obviously if it's always open this shouldn't happen :) *)
        ""
