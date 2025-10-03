module Duets.Entities.World

open Aether
open Duets.Common
open Duets.Entities

/// Creates an empty world.
let empty = { Cities = Map.empty }

/// Creates a new world with the given cities inside of it.
let create (cities: City list) =
    { Cities = cities |> List.map (fun c -> c.Id, c) |> Map.ofList }

[<RequireQualifiedAccess>]
module Graph =
    /// Creates an empty, invalid graph. Since starting node is a mandatory
    /// field, this will create a graph with a starting node pointing to nowhere.
    /// Only use for cases where you **really** don't have the first node without
    /// having access to the graph first.
    let empty =
        { StartingNode = "INVALID"
          Nodes = Map.empty
          Connections = Map.empty }

    /// Sets the starting node of the given graph to the given node ID.
    let setStartingNode nodeId graph = { graph with StartingNode = nodeId }

    /// Creates a graph with the given starting node as the starting point, that
    /// node as the only node available and no connections.
    let from (node: Node<'a>) =
        { StartingNode = node.Id
          Nodes = [ (node.Id, node.Content) ] |> Map.ofList
          Connections = Map.empty }

    /// Creates a graph with the given nodes as the only nodes available and no
    /// connections. Chooses the head of the node list as the starting node. If
    /// an empty list is given, the function fails.
    let fromMany (nodes: Node<'a> list) =
        let startingNode = nodes |> List.head

        { StartingNode = startingNode.Id
          Nodes = nodes |> List.map (fun n -> n.Id, n.Content) |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given graph.
    let addNode (node: Node<'a>) =
        Optic.map Lenses.World.Graph.nodes_ (Map.add node.Id node.Content)

    /// Adds a connection from the first node ID to the second in the given
    /// direction. Also adds a second connection from the second node ID to
    /// the first as the opposite direction of the given one, so that if the
    /// given direction is north then the second connection will be generated
    /// in the south.
    let connect fromNodeId toNodeId direction graph =
        let oppositeDirection =
            match direction with
            | North -> South
            | NorthEast -> SouthWest
            | East -> West
            | SouthEast -> NorthWest
            | South -> North
            | SouthWest -> NorthEast
            | West -> East
            | NorthWest -> SouthEast

        graph
        |> Optic.map
            (Lenses.World.Graph.nodeConnections_ fromNodeId)
            (Map.add direction toNodeId)
        |> Optic.map
            (Lenses.World.Graph.nodeConnections_ toNodeId)
            (Map.add oppositeDirection fromNodeId)

    /// Adds many connections to the given graph.
    let connectMany connections graph =
        connections
        |> List.fold
            (fun g (fromNodeId, toNodeId, direction) ->
                connect fromNodeId toNodeId direction g)
            graph

    /// Returns all the nodes in the graph.
    let nodes graph = graph.Nodes |> List.ofMapValues

    /// Changes the node with the given ID to the given node.
    let changeNode nodeId node graph =
        { graph with
            Nodes = Map.change nodeId node graph.Nodes }

[<RequireQualifiedAccess>]
module Node =
    /// Creates a new node with an auto-generated ID and the given content.
    let create id (content: 'a) = { Id = id; Content = content }

[<RequireQualifiedAccess>]
module Place =
    /// Creates a place with the given initial room and no exits.
    let create name quality placeType rooms zoneId =
        let inferredId = Identity.Reproducible.create name

        { Id = inferredId
          Name = name
          Exits = Map.empty
          Quality = quality
          PlaceType = placeType
          PromptContext = ""
          OpeningHours = PlaceOpeningHours.AlwaysOpen
          Rooms = rooms
          ZoneId = zoneId }

    /// Attaches a prompt context to the given place.
    let attachContext context (place: Place) =
        { place with PromptContext = context }

    /// Adds an exit to the given place.
    let addExit originRoomId targetPlaceId place =
        { place with
            Exits = Map.add originRoomId targetPlaceId place.Exits }

    /// Changes the opening hours to a certain days and day moments.
    let changeOpeningHours openingHours place =
        { place with
            OpeningHours = openingHours }

    /// Changes a specific room in the place.
    let changeRoom roomId mapping place =
        { place with
            Rooms = Graph.changeNode roomId mapping place.Rooms }

    module Type =
        /// Transforms a PlaceType into its index counterpart.
        let toIndex placeType =
            match placeType with
            | Airport -> PlaceTypeIndex.Airport
            | Bar -> PlaceTypeIndex.Bar
            | Bookstore -> PlaceTypeIndex.Bookstore
            | Cafe -> PlaceTypeIndex.Cafe
            | Casino -> PlaceTypeIndex.Casino
            | ConcertSpace _ -> PlaceTypeIndex.ConcertSpace
            | Gym -> PlaceTypeIndex.Gym
            | Home -> PlaceTypeIndex.Home
            | Hotel _ -> PlaceTypeIndex.Hotel
            | Hospital -> PlaceTypeIndex.Hospital
            | MetroStation -> PlaceTypeIndex.MetroStation
            | MerchandiseWorkshop -> PlaceTypeIndex.MerchandiseWorkshop
            | RadioStudio _ -> PlaceTypeIndex.RadioStudio
            | RehearsalSpace _ -> PlaceTypeIndex.RehearsalSpace
            | Restaurant -> PlaceTypeIndex.Restaurant
            | Street -> PlaceTypeIndex.Street
            | Studio _ -> PlaceTypeIndex.Studio

[<RequireQualifiedAccess>]
module Street =
    /// Creates a street with the given name, type and places.
    let create name streetType =
        let inferredId = Identity.Reproducible.create name

        { Id = inferredId
          Name = name
          PromptContext = ""
          Type = streetType
          Places = [] }

    /// Attaches a prompt context to the given street.
    let attachContext context (street: Street) =
        { street with PromptContext = context }

    /// Adds a place to the given street.
    let addPlace place street =
        { street with
            Places = place :: street.Places }

    /// Adds many places to the given street.
    let addPlaces places street =
        { street with
            Places = places @ street.Places }

    /// Attempts to find a place of the given type in the street.
    let tryFindPlaceOfType placeType street =
        street.Places |> List.tryFind (fun place -> place.PlaceType = placeType)

[<RequireQualifiedAccess>]
module Zone =
    /// Creates a zone with the given name and an ID based on it.
    let create name =
        let inferredId = Identity.Reproducible.create name

        { Id = inferredId.ToString()
          Name = name
          Descriptors = []
          MetroStations = Map.empty
          Streets = Graph.empty }

    /// Adds a descriptor to the given zone.
    let addDescriptor descriptor zone =
        { zone with
            Descriptors = descriptor :: zone.Descriptors }

    /// Adds a street to the given zone.
    let addStreet (street: Node<Street>) (zone: Zone) =
        (*
        In order for streets to work at all we need to have synthetic rooms added
        to it so that we can reference them through regular coordinates. However,
        since we also allow streets to be navigated through its splits (in case
        the street is a split street), we need to add multiple rooms to cover
        these cases.
        *)
        let streetRoomGraph =
            match street.Content.Type with
            | OneWay ->
                Node.create
                    "0"
                    { RoomType = RoomType.Street
                      RequiredItemsForEntrance = None }
                |> Graph.from
            | Split(throughDirection, splits) ->
                let graph = Graph.empty |> Graph.setStartingNode "0"

                let connections =
                    Seq.init (splits - 1) (fun i -> i, i + 1)
                    |> Seq.map (fun (fromNode, toNode) ->
                        (fromNode.ToString(),
                         toNode.ToString(),
                         throughDirection))
                    |> List.ofSeq

                Seq.init splits id
                |> Seq.fold
                    (fun outerGraph index ->
                        Graph.addNode
                            (Node.create
                                (index.ToString())
                                { RoomType = RoomType.Street
                                  RequiredItemsForEntrance = None })
                            outerGraph)
                    graph
                |> Graph.connectMany connections

        (*
        When looking up places we need to perform the lookup inside a street.
        That means that if the player goes out to the street, it won't be found
        since streets won't contain themselves. To go around this, we add a
        synthetic, empty street place to the street.
        *)
        let syntheticStreetPlace =
            Place.create
                street.Content.Name
                100<quality>
                PlaceType.Street
                streetRoomGraph
                zone.Id
            |> Place.attachContext street.Content.PromptContext

        let street =
            { street with
                Content.Places = syntheticStreetPlace :: street.Content.Places }

        { zone with
            Streets = Graph.addNode street zone.Streets }

    /// Connects two streets in the given zone.
    let connectStreets fromStreet toStreet direction zone =
        { zone with
            Streets = Graph.connect fromStreet toStreet direction zone.Streets }

    /// Adds a metro station to the given zone.
    let addMetroStation station zone =
        { zone with
            MetroStations =
                station.Lines
                |> List.fold
                    (fun acc line -> Map.add line station acc)
                    zone.MetroStations }

[<RequireQualifiedAccess>]
module City =
    let private allPlacesInZone zone =
        zone.Streets.Nodes
        |> Map.fold
            (fun places _ street ->
                let street = zone.Streets.Nodes |> Map.find street.Id

                street.Places
                |> List.fold
                    (fun (outerPlaces: List<_>) place ->
                        (zone, street, place) :: outerPlaces)
                    places)
            List.empty

    let private indexZoneByPlacesTypes zone index =
        allPlacesInZone zone
        |> List.fold
            (fun outerIndex (_, _, place) ->
                let mapKey = Place.Type.toIndex place.PlaceType

                Map.change
                    mapKey
                    (function
                    | Some list -> list @ [ place.Id ] |> Some
                    | None -> [ place.Id ] |> Some)
                    outerIndex)
            index

    let private indexStreets zone index =
        zone.Streets.Nodes
        |> Map.fold
            (fun outerIndex streetId street ->
                outerIndex |> Map.add streetId street)
            index

    let private indexZonePlaces zone index =
        // Index a synthetic "Street" place type for each street so that we have
        // a way to put the player somewhere when they enter a street. This is
        // important for storing the current location in the save file.
        let index =
            zone.Streets.Nodes
            |> Map.fold
                (fun outerIndex _ street ->
                    outerIndex |> Map.add street.Id (zone.Id, street.Id, "0"))
                index

        allPlacesInZone zone
        |> List.fold
            (fun outerIndex (zone, street, place) ->
                outerIndex |> Map.add place.Id (zone.Id, street.Id, place.Id))
            index

    /// Creates a city with only one initial starting node.
    let create id costOfLiving utcOffset zone =
        { Id = id
          PlaceByTypeIndex = indexZoneByPlacesTypes zone Map.empty
          PlaceIndex = indexZonePlaces zone Map.empty
          StreetIndex = indexStreets zone Map.empty
          MetroLines = Map.empty
          CostOfLiving = costOfLiving
          Zones = [ zone.Id, zone ] |> Map.ofList
          Timezone = Utc(utcOffset) }

    /// Adds a zone to the given city.
    let addZone zone city =
        Optic.map
            Lenses.World.City.placeByTypeIndex_
            (indexZoneByPlacesTypes zone)
            city
        |> Optic.map Lenses.World.City.placeIndex_ (indexZonePlaces zone)
        |> Optic.map Lenses.World.City.streetIndex_ (indexStreets zone)
        |> Optic.map Lenses.World.City.zones_ (Map.add zone.Id zone)

    /// Adds a metro line to the given city.
    let addMetroLine (metroLine: MetroLine) city =
        { city with
            MetroLines = Map.add metroLine.Id metroLine city.MetroLines }

[<RequireQualifiedAccess>]
module Room =
    /// Creates a room with the given type and no required items for entrance.
    let create roomType =
        { RoomType = roomType
          RequiredItemsForEntrance = None }

    /// Adds a required item for entrance to the given room.
    let changeRequiredItemForEntrance requiredItems room =
        { room with
            RequiredItemsForEntrance = Some requiredItems }

    /// Removes the required item for entrance from the given room.
    let removeRequiredItemForEntrance room =
        { room with
            RequiredItemsForEntrance = None }

[<RequireQualifiedAccess>]
module Coordinates =
    /// Returns the PlaceCoordinates of the given room coordinates.
    let toPlaceCoordinates (coords: RoomCoordinates) : PlaceCoordinates =
        let cityId, placeId, _ = coords
        cityId, placeId
