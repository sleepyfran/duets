namespace Duets.Entities

open System
open Duets.Entities

[<AutoOpen>]
module WorldTypes =
    /// ID for a zone in a city.
    type ZoneId = ZoneId of Identity

    /// Defines a zone inside of a city where places are contained.
    type Zone = { Id: ZoneId; Name: string }

    /// Defines the opening hours of one place, which can be always open (24/7)
    /// or a selection of day moments for some given days.
    [<RequireQualifiedAccess>]
    type PlaceOpeningHours =
        | AlwaysOpen
        | OpeningHours of days: DayOfWeek list * dayMoments: DayMoment list

    /// Defines all the different types of places that the game supports.
    type PlaceType =
        | Airport
        | Bar of Shop
        | Cafe of Shop
        | ConcertSpace of ConcertSpace
        | Home
        | Hospital
        | RehearsalSpace of RehearsalSpace
        | Restaurant of Shop
        | Studio of Studio

    /// Re-defines all types of places above but without its content, to be able
    /// to reference them on a map.
    [<RequireQualifiedAccess>]
    type PlaceTypeIndex =
        | Airport
        | Bar
        | Cafe
        | ConcertSpace
        | Home
        | Hospital
        | RehearsalSpace
        | Restaurant
        | Studio

    /// ID for a place in the game world.
    type PlaceId = PlaceId of Identity

    /// Defines a place inside of the game world, which wraps a given space
    /// (could be any inside space like a rehearsal place or a concert hall), the
    /// rooms that the place itself contains and the exits that connect that
    /// place with the outside.
    type Place =
        { Id: PlaceId
          Name: string
          Quality: Quality
          Type: PlaceType
          OpeningHours: PlaceOpeningHours
          Zone: Zone }

    /// ID for a city in the game world, which declared every possible city
    /// available in the game.
    type CityId =
        | Prague
        | Madrid

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        { Id: CityId
          PlaceByTypeIndex: Map<PlaceTypeIndex, PlaceId list>
          PlaceIndex: Map<PlaceId, Place>
          ZoneIndex: Map<ZoneId, PlaceId list> }

    /// Defines a position in the world.
    type WorldCoordinates = CityId * PlaceId

    /// Contains all the items that a specific location has.
    type WorldItems = Map<WorldCoordinates, Item list>

    /// Defines the game world which contains all cities.
    type World = { Cities: Map<CityId, City> }
    
    // Defines all possible errors why the entrance to a place might be denied.
    [<RequireQualifiedAccess>]
    type PlaceEntranceError =
        | CannotEnterOutsideOpeningHours
        | CannotEnterWithoutRental
