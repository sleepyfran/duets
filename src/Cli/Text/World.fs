[<RequireQualifiedAccess>]
module Cli.Text.World

open Entities

let placeDescription (place: Place) = $"You are at {Styles.place place.Name}"

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Studio -> "Studio"

let movedTo (place: Place) = $"You make your way to {place.Name}..."
