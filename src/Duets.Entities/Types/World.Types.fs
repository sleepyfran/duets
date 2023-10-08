namespace Duets.Entities

open System
open Duets.Entities

[<AutoOpen>]
module WorldTypes =
    /// Defines all possible cardinal directions that can be used to traverse
    /// the room map.
    type Direction =
        | North
        | NorthEast
        | East
        | SouthEast
        | South
        | SouthWest
        | West
        | NorthWest

    /// Defines all connections that a node has in each of its directions.
    type NodeConnections = Map<Direction, NodeId>

    /// Defines the relationship between a node ID and its content.
    type Node<'a> = { Id: NodeId; Content: 'a }

    /// Represents a graph that can be used to create connections between
    /// different nodes in each of the available directions. All connected
    /// nodes are considered undirected, meaning that if A and B are connected
    /// then the player can go from A to B and B to A. These connections are
    /// made in the opposite direction, so if A connects through the North to
    /// B, then B connects through the South to A.
    type Graph<'a> =
        { StartingNode: NodeId
          Nodes: Map<NodeId, 'a>
          Connections: Map<NodeId, NodeConnections> }

    /// ID for a zone in a city.
    type ZoneId = string

    /// Defines a zone inside of a city where places are contained.
    type Zone = { Id: ZoneId; Name: string }

    /// Defines the opening hours of one place, which can be always open (24/7)
    /// or a selection of day moments for some given days.
    [<RequireQualifiedAccess>]
    type PlaceOpeningHours =
        | AlwaysOpen
        | OpeningHours of days: DayOfWeek list * dayMoments: DayMoment list

    // Defines all types of rooms that player can be in.
    [<RequireQualifiedAccess>]
    type RoomType =
        | Backstage
        | Bar
        | Bedroom
        | BoardingGate
        | Cafe
        | CasinoFloor
        | ChangingRoom
        | Gym
        | Kitchen
        | LivingRoom
        | Lobby
        | MasteringRoom
        | RecordingRoom
        | RehearsalRoom
        | Restaurant of RestaurantCuisine
        | SecurityControl
        | Stage

    /// Defines which items are required to enter a given room from a given
    /// node.
    type RequireItemsForEntrance =
        { ComingFrom: NodeId; Items: Item list }

    /// Defines a room which is contained inside of a place.
    type Room =
        { RoomType: RoomType
          RequiredItemsForEntrance: RequireItemsForEntrance option }

    /// Defines all the different types of places that the game supports.
    type PlaceType =
        | Airport
        | Bar
        | Cafe
        | Casino
        | ConcertSpace of ConcertSpace
        | Gym
        | Home
        | Hotel of Hotel
        | Hospital
        | RehearsalSpace of RehearsalSpace
        | Restaurant
        | Studio of Studio

    /// Re-defines all types of places above but without its content, to be able
    /// to reference them on a map.
    [<RequireQualifiedAccess>]
    type PlaceTypeIndex =
        | Airport
        | Bar
        | Cafe
        | Casino
        | ConcertSpace
        | Gym
        | Home
        | Hotel
        | Hospital
        | RehearsalSpace
        | Restaurant
        | Studio

    /// Defines a place inside of the game world, which wraps a given space
    /// (could be any inside space like a rehearsal place or a concert hall), the
    /// rooms that the place itself contains and the exits that connect that
    /// place with the outside.
    type Place =
        { Id: PlaceId
          Name: string
          Quality: Quality
          PlaceType: PlaceType
          OpeningHours: PlaceOpeningHours
          Rooms: Graph<Room>
          Zone: Zone }

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        {
            Id: CityId
            PlaceByTypeIndex: Map<PlaceTypeIndex, PlaceId list>
            PlaceIndex: Map<PlaceId, Place>
            /// Modifier that will be used to compute the final prices of things
            /// like rent, wages and food.
            CostOfLiving: float
            ZoneIndex: Map<ZoneId, PlaceId list>
        }

    /// Defines the game world which contains all cities.
    type World = { Cities: Map<CityId, City> }

    // Defines all possible errors why the entrance to a place might be denied.
    [<RequireQualifiedAccess>]
    type PlaceEntranceError =
        | CannotEnterOutsideOpeningHours
        | CannotEnterWithoutRental

    /// Defines all possible errors why the entrance to a room might be denied.
    [<RequireQualifiedAccess>]
    type RoomEntranceError =
        | CannotEnterStageOutsideConcert
        | CannotEnterBackstageOutsideConcert
        | CannotEnterHotelRoomWithoutBooking
        | CannotEnterWithoutRequiredItems of items: Item list
