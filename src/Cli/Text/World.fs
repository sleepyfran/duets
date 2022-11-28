[<RequireQualifiedAccess>]
module Cli.Text.World

open Entities

let placeDescription (place: Place) = $"You are at {Styles.place place.Name}"

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Airport -> "Airport"
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Studio -> "Studio"

let placeWithZone (place: Place) =
    $"{Styles.place place.Name} ({place.Zone.Name})"

let movedTo (place: Place) = $"You make your way to {place.Name}..."
