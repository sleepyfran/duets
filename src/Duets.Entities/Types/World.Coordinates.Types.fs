namespace Duets.Entities

[<AutoOpen>]
module WorldCoordinatesTypes =
    /// Unique ID of a node.
    type NodeId = int

    /// ID for a place in the game world.
    type PlaceId = string

    /// Defines a position in the world, including up to the room inside of
    /// the place.
    type RoomCoordinates = CityId * PlaceId * NodeId

    /// Simplified coordinates that only contain the city and the place.
    type PlaceCoordinates = CityId * PlaceId
