namespace Duets.Entities

open System
open System.Collections.Generic
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
        | Platform
        | ReadingRoom
        | RecordingRoom
        | RehearsalRoom
        | Restaurant of RestaurantCuisine
        | SecurityControl
        | Stage
        | Street
        | Workshop

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
        | Bookstore
        | Cafe
        | Casino
        | ConcertSpace of ConcertSpace
        | Gym
        | Home
        | Hotel of Hotel
        | Hospital
        | MerchandiseWorkshop
        | MetroStation
        | RehearsalSpace of RehearsalSpace
        | Restaurant
        | Street
        | Studio of Studio

    /// Re-defines all types of places above but without its content, to be able
    /// to reference them on a map.
    [<RequireQualifiedAccess>]
    type PlaceTypeIndex =
        | Airport
        | Bar
        | Bookstore
        | Cafe
        | Casino
        | ConcertSpace
        | Gym
        | Home
        | Hotel
        | Hospital
        | MerchandiseWorkshop
        | MetroStation
        | RehearsalSpace
        | Restaurant
        | Street
        | Studio

    /// Defines a place inside of the game world, which wraps a given space
    /// (could be any inside space like a rehearsal place or a concert hall), the
    /// rooms that the place itself contains and the exits that connect that
    /// place with the outside.
    type Place =
        {
            Id: PlaceId
            Name: string
            /// Map of nodes that are considered exist in the room graph with the
            /// ID of the street that it exits to if the player follows it.
            Exits: Map<NodeId, StreetId>
            Quality: Quality
            PlaceType: PlaceType
            OpeningHours: PlaceOpeningHours
            Rooms: Graph<Room>
            ZoneId: ZoneId
        }

        interface IComparer<Place> with
            member this.Compare(x: Place, y: Place) = String.Compare(x.Id, y.Id)

    /// The type of the streets defines whether the street will be displayed
    /// as a single street (all places will be available at one upon arrival)
    /// or split, where the places will be distributed based on the specified
    /// number of splits.
    type StreetType =
        | OneWay
        | Split of throughDirection: Direction * splits: int

    /// Defines a street inside of a zone where places are contained.
    type Street =
        { Id: StreetId
          Name: string
          Type: StreetType
          Places: Place list }

    /// Defines a zone inside of a city where places are contained.
    type Zone =
        {
            Id: ZoneId
            Name: string
            /// Descriptors that can describe the zone, like "Bohemian" or "Luxurious".
            /// The generator will do its best to generate a description based on the
            /// descriptors provided, but at most this should include 3 descriptors
            /// to avoid having conflicting descriptors.
            Descriptors: ZoneDescriptor list
            Streets: Graph<Street>
            /// Stations that these zone have per line.
            MetroStations: Map<MetroLineId, MetroStation>
        }

    [<Measure>]
    type utcOffset

    /// Defines a timezone in the world as an offset from UTC.
    type Timezone = Utc of int<utcOffset>

    [<Measure>]
    type costOfLiving

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        {
            Id: CityId
            PlaceByTypeIndex: Map<PlaceTypeIndex, PlaceId list>
            PlaceIndex: Map<PlaceId, ZonedPlaceCoordinates>
            StreetIndex: Map<StreetId, Street>
            MetroLines: Map<MetroLineId, MetroLine>
            /// Modifier that will be used to compute the final prices of things
            /// like rent, wages and food.
            CostOfLiving: float<costOfLiving>
            Timezone: Timezone
            Zones: Map<ZoneId, Zone>
        }

    /// Defines a distance between two cities in km.
    type CityConnectionDistance = int<km>

    /// Defines which connection is available between two cities.
    type CityConnectionType =
        | Air
        | Sea
        | Road

    /// Defines which connections are available between two cities.
    type CityConnections = CityConnectionType list

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
