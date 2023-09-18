[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.World

open Duets.Cli.Text
open Duets.Entities

let placeDescription (place: Place) (roomType: RoomType) =
    (place, roomType)
    ||> match place.PlaceType with
        | PlaceType.Airport -> Airport.description
        | PlaceType.Bar -> Bar.description
        | PlaceType.Cafe -> Cafe.description
        | PlaceType.Casino -> Casino.description
        | PlaceType.ConcertSpace _ -> ConcertSpace.description
        | PlaceType.Gym -> Gym.description
        | PlaceType.Home -> Home.description
        | PlaceType.Hospital -> Hospital.description
        | PlaceType.Hotel _ -> Hotel.description
        | PlaceType.RehearsalSpace _ -> RehearsalSpace.description
        | PlaceType.Restaurant -> Restaurant.description
        | PlaceType.Studio studio -> Studio.description studio

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.Cafe -> "Café"
    | PlaceTypeIndex.Casino -> "Casino"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Gym -> "Gym"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.Hotel -> "Hotel"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Restaurant -> "Restaurant"
    | PlaceTypeIndex.Studio -> "Studio"

let roomName (room: RoomType) =
    match room with
    | RoomType.Backstage -> "backstage"
    | RoomType.Bar -> "bar area"
    | RoomType.Bedroom -> "bedroom"
    | RoomType.Cafe -> "cafe area"
    | RoomType.CasinoFloor -> "casino floor"
    | RoomType.ChangingRoom -> "changing room"
    | RoomType.Gym -> "gym"
    | RoomType.Kitchen -> "kitchen"
    | RoomType.LivingRoom -> "living room"
    | RoomType.Lobby -> "lobby"
    | RoomType.MasteringRoom -> "mastering room"
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant _ -> "restaurant"
    | RoomType.SecurityControl -> "security control"
    | RoomType.Stage -> "stage"

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
    $"{Styles.place place.Name} ({place.Zone.Name})"
