namespace Duets.Entities

[<AutoOpen>]
module WorldCoordinatesTypes =
    /// Unique ID of a node.
    type NodeId = string

    /// ID for a room in a place in the game world.
    type RoomId = NodeId

    /// ID for a place in the game world.
    type PlaceId = string

    /// ID for a street inside a zone.
    type StreetId = NodeId

    /// ID for a zone in a city.
    type ZoneId = string

    /// Defines a position in the world, including up to the room inside of
    /// the place.
    type RoomCoordinates = CityId * PlaceId * RoomId

    /// Defines a position inside a specific zone that can resolve a place. These
    /// coordinates are used to reference places inside a city, but they can't
    /// resolve a place globally due to the lack of a city ID.
    type ZonedPlaceCoordinates = ZoneId * StreetId * PlaceId

    /// Simplified coordinates that only contain the city and the place.
    type PlaceCoordinates = CityId * PlaceId
