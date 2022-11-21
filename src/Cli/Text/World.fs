[<RequireQualifiedAccess>]
module Cli.Text.World

open Entities

let placeDescription (place: Place) =
    match place.Type with
    | Bar _ -> $"You are in the bar {Styles.place place.Name}"
    | ConcertSpace _ -> $"You are inside of the {Styles.place place.Name} venue"
    | Home -> "You are at home"
    | Hospital -> $"You are inside the hospital {Styles.place place.Name}"
    | RehearsalSpace _ ->
        $"You are in the rehearsal space {Styles.place place.Name}"
    | Studio _ -> $"You are in the studio {Styles.place place.Name}"

let placeTypeName (placeType: PlaceTypeIndex) =
    match placeType with
    | PlaceTypeIndex.Bar -> "Bar"
    | PlaceTypeIndex.ConcertSpace -> "Concert space"
    | PlaceTypeIndex.Home -> "Home"
    | PlaceTypeIndex.Hospital -> "Hospital"
    | PlaceTypeIndex.RehearsalSpace -> "Rehearsal space"
    | PlaceTypeIndex.Studio -> "Studio"

let movedTo (place: Place) = $"You make your way to {place.Name}..."
