[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.World

open Duets.Cli.Text
open Duets.Entities

let placeDescription (place: Place) (roomType: RoomType) =
    (place, roomType)
    ||> match place.Type with
        | PlaceType.Airport -> Airport.description
        | PlaceType.Bar _ -> Bar.description
        | PlaceType.Cafe _ -> Cafe.description
        | PlaceType.ConcertSpace _ -> ConcertSpace.description
        | PlaceType.Home -> Home.description
        | PlaceType.Hospital -> Hospital.description
        | PlaceType.RehearsalSpace _ -> RehearsalSpace.description
        | PlaceType.Restaurant _ -> Restaurant.description
        | PlaceType.Studio studio -> Studio.description studio

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.Cafe -> "Café"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Restaurant -> "Restaurant"
    | PlaceTypeIndex.Studio -> "Studio"

let roomName (room: RoomType) =
    match room with
    | RoomType.Backstage -> "backstage"
    | RoomType.Bar -> "bar area"
    | RoomType.Cafe -> "cafe area"
    | RoomType.Bedroom -> "bedroom"
    | RoomType.Kitchen -> "kitchen"
    | RoomType.LivingRoom -> "living room"
    | RoomType.Lobby -> "lobby"
    | RoomType.MasteringRoom -> "mastering room"
    | RoomType.RecordingRoom -> "recording room"
    | RoomType.RehearsalRoom -> "rehearsal room"
    | RoomType.Restaurant -> "restaurant"
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

let movedTo (place: Place) = $"You make your way to {place.Name}..."
