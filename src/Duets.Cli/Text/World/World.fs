[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.World

open Duets.Cli.Text
open Duets.Entities

let placeNameWithOpeningInfo placeDetails currentlyOpen =
    match currentlyOpen with
    | true -> placeDetails
    | false -> Styles.faded $"{placeDetails} - Closed"

let placeNameWithOpeningInfo' (place: Place) currentlyOpen =
    placeNameWithOpeningInfo place.Name currentlyOpen

let placeDescription (place: Place) (roomType: RoomType) =
    (place, roomType)
    ||> match place.PlaceType with
        | PlaceType.Airport -> Airport.description
        | PlaceType.Bar -> Bar.description
        | PlaceType.Bookstore -> Bookstore.description
        | PlaceType.Cafe -> Cafe.description
        | PlaceType.Casino -> Casino.description
        | PlaceType.ConcertSpace _ -> ConcertSpace.description
        | PlaceType.Gym -> Gym.description
        | PlaceType.Home -> Home.description
        | PlaceType.Hospital -> Hospital.description
        | PlaceType.Hotel _ -> Hotel.description
        | PlaceType.MerchandiseWorkshop -> MerchandiseWorkshop.description
        | PlaceType.RehearsalSpace _ -> RehearsalSpace.description
        | PlaceType.Restaurant -> Restaurant.description
        | PlaceType.RadioStudio _ -> RadioStudio.description
        | PlaceType.Studio studio -> Studio.description studio

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.Bookstore -> "Bookstore"
    | PlaceTypeIndex.Cafe -> "Café"
    | PlaceTypeIndex.Casino -> "Casino"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Gym -> "Gym"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.Hotel -> "Hotel"
    | PlaceTypeIndex.MerchandiseWorkshop -> "Merchandise workshop"
    | PlaceTypeIndex.RadioStudio -> "Radio Studio"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Restaurant -> "Restaurant"
    | PlaceTypeIndex.Studio -> "Studio"

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
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.ReadingRoom -> "reading room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant _ -> "restaurant"
    | RoomType.SecurityControl -> "security control"
    | RoomType.Stage -> "stage"
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

let placeWithZone (place: Place) =
    let baseInfo = $"{Styles.place place.Name} ({place.Zone.Name})"

    match place.PlaceType with
    | PlaceType.Studio studio ->
        $"{baseInfo} ({studio.PricePerSong |> Styles.money}/song)"
    | _ -> baseInfo

let placeArrivalMessage place roomType =
    match place.PlaceType with
    | PlaceType.RehearsalSpace _ -> RehearsalSpace.arrivalMessage roomType
    | _ -> None

let placeClosedError (place: Place) =
    $"{place.Name} is currently closed. Try again during their opening hours"
    |> Styles.error

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
