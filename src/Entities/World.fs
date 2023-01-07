module Entities.World

open Aether
open Entities
open System

/// Creates an empty world.
let empty = { Cities = Map.empty }

/// Creates a new world with the given cities inside of it.
let create (cities: City list) =
    { Cities = cities |> List.map (fun c -> c.Id, c) |> Map.ofList }

[<RequireQualifiedAccess>]
module Place =
    module OpeningHours =
        /// List of days from Monday to Friday.
        let weekday =
            [ DayOfWeek.Monday
              DayOfWeek.Tuesday
              DayOfWeek.Wednesday
              DayOfWeek.Thursday
              DayOfWeek.Wednesday
              DayOfWeek.Friday ]

        /// List of every day of the week.
        let everyDay =
            [ DayOfWeek.Monday
              DayOfWeek.Tuesday
              DayOfWeek.Wednesday
              DayOfWeek.Thursday
              DayOfWeek.Wednesday
              DayOfWeek.Friday
              DayOfWeek.Saturday
              DayOfWeek.Sunday ]

    /// Creates a place with the given initial room and no exits.
    let create id name quality placeType zone =
        { Id = PlaceId id
          Name = name
          Quality = quality
          Type = placeType
          OpeningHours = PlaceOpeningHours.AlwaysOpen
          Zone = zone }

    /// Changes the opening hours to a certain days and day moments.
    let changeOpeningHours openingHours place =
        { place with OpeningHours = openingHours }

[<RequireQualifiedAccess>]
module City =
    let private addToPlaceByTypeIndex place index =
        let mapKey =
            match place.Type with
            | Airport -> PlaceTypeIndex.Airport
            | Bar _ -> PlaceTypeIndex.Bar
            | Cafe _ -> PlaceTypeIndex.Cafe
            | ConcertSpace _ -> PlaceTypeIndex.ConcertSpace
            | Home -> PlaceTypeIndex.Home
            | Hospital -> PlaceTypeIndex.Hospital
            | RehearsalSpace _ -> PlaceTypeIndex.RehearsalSpace
            | Restaurant _ -> PlaceTypeIndex.Restaurant
            | Studio _ -> PlaceTypeIndex.Studio

        Map.change
            mapKey
            (function
            | Some list -> list @ [ place.Id ] |> Some
            | None -> [ place.Id ] |> Some)
            index

    let private addToZoneIndex place =
        Map.change place.Zone.Id (function
            | Some list -> list @ [ place.Id ] |> Some
            | None -> [ place.Id ] |> Some)

    /// Creates a city with only one initial starting node.
    let create id place =
        { Id = id
          PlaceByTypeIndex = addToPlaceByTypeIndex place Map.empty
          PlaceIndex = [ (place.Id, place) ] |> Map.ofList
          ZoneIndex = addToZoneIndex place Map.empty }

    /// Adds a new place to the city.
    let addPlace place city =
        Optic.map
            Lenses.World.City.placeByTypeIndex_
            (addToPlaceByTypeIndex place)
            city
        |> Optic.map Lenses.World.City.placeIndex_ (Map.add place.Id place)
        |> Optic.map Lenses.World.City.zoneIndex_ (addToZoneIndex place)
