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

let placeWithZone (place: Place) =
    $"{Styles.place place.Name} ({place.Zone.Name})"

let movedTo (place: Place) = $"You make your way to {place.Name}..."
