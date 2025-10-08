module rec Duets.Simulation.Navigation.Pathfinding

open Duets.Common
open Duets.Entities
open Duets.Simulation

type PathAction =
    | GoOut of fromPlace: Place * toStreet: Street
    | Enter of fromStreet: Street * toPlace: Place
    | TakeMetro of
        fromStation: MetroStation *
        toStation: MetroStation *
        throughLine: MetroLineId
    | Walk of fromStreet: Street * toStreet: Street * through: Direction

/// Creates the directions that the player needs to follow in order to reach
/// the given destination node from the given origin node in the given city.
/// If not connection can be found (due to either of the places not being in the
/// provided city or an oopsie on the world data), then None is returned.
let directionsToNode cityId originNode destinationNode =
    let nodesBelongToCity =
        Queries.World.nodesBelongToCity cityId originNode destinationNode

    match nodesBelongToCity with
    | false -> None
    | true -> directionsToNode' [] cityId originNode destinationNode

let private directionsToNode' directions cityId currentNode destinationNode =
    if currentNode = destinationNode then
        Some(List.rev directions)
    else
        let currentPlace = Queries.World.placeInCityById cityId currentNode
        let targetPlace = Queries.World.placeInCityById cityId destinationNode

        match currentPlace.PlaceType with
        | MetroStation ->
            goToStreetByMetro
                cityId
                currentPlace
                targetPlace
                destinationNode
                directions
        | Street ->
            if currentPlace.Id = targetPlace.StreetId then
                (* The place we're looking for is in the same street, enter directly. *)
                enterPlaceInSameStreet
                    cityId
                    currentPlace
                    targetPlace
                    directions
            else if currentPlace.ZoneId = targetPlace.ZoneId then
                (* The places are in the same zone, find the shortest path through the street graph. *)
                findConnectionInSameZone
                    cityId
                    currentPlace
                    targetPlace
                    destinationNode
                    directions
            else
                (* The places are in complete different zones, find the closest metro station to get there. *)
                findClosestMetroStation
                    cityId
                    currentPlace
                    destinationNode
                    directions
        | _ ->
            // TODO: Support multiple exits.
            let connectedStreet =
                currentPlace.Exits
                |> Map.head
                |> Queries.World.streetById cityId

            directionsToNode'
                (GoOut(currentPlace, connectedStreet) :: directions)
                cityId
                connectedStreet.Id
                destinationNode

let private enterPlaceInSameStreet cityId currentPlace targetPlace directions =
    let street = Queries.World.streetById cityId currentPlace.Id

    directionsToNode'
        (Enter(street, targetPlace) :: directions)
        cityId
        targetPlace.Id
        targetPlace.Id

let private goToStreetByMetro
    cityId
    currentPlace
    targetPlace
    destinationNode
    directions
    =
    let currentZone = Queries.World.zoneInCityById cityId currentPlace.ZoneId

    let targetZone = Queries.World.zoneInCityById cityId targetPlace.ZoneId

    let targetMetroStations = targetZone.MetroStations |> List.ofMap

    let targetMetroStation =
        targetMetroStations
        |> List.tryFind (fun (lineId, _) ->
            currentZone.MetroStations |> Map.containsKey lineId)

    match targetMetroStation with
    | Some(targetLine, targetMetroStation) ->
        takeMetroThroughConnectingLine
            cityId
            currentZone
            targetLine
            targetMetroStation
            destinationNode
            directions
    | None ->
        takeMetroThroughTransferLine
            cityId
            currentZone
            targetZone
            destinationNode
            directions

let private takeMetroThroughConnectingLine
    cityId
    (currentZone: Zone)
    targetLine
    targetMetroStation
    destinationNode
    directions
    =
    let currentMetroStation =
        Queries.Metro.tryStationFromZone' cityId currentZone.Id targetLine
        |> Option.get (* We've already verified above that we do have a connecting line, so should be safe... Famous words :^) *)

    let targetMetroPlace =
        Queries.World.placeInCityById cityId targetMetroStation.PlaceId

    let connectingExitStreet =
        Queries.World.streetById cityId targetMetroStation.LeavesToStreet

    (* Directions are in reverse order until we find the path. *)
    let nextDirections =
        GoOut(targetMetroPlace, connectingExitStreet)
        :: TakeMetro(currentMetroStation, targetMetroStation, targetLine)
        :: directions

    directionsToNode'
        nextDirections
        cityId
        targetMetroStation.LeavesToStreet
        destinationNode

let private takeMetroThroughTransferLine
    cityId
    currentZone
    targetZone
    destinationNode
    directions
    =
    match bfsMetroStations cityId currentZone targetZone with
    | Some path ->
        let _, targetMetroStation, _ = List.head path

        let targetMetroPlace =
            Queries.World.placeInCityById cityId targetMetroStation.PlaceId

        let connectingExitStreet =
            Queries.World.streetById cityId targetMetroStation.LeavesToStreet

        (* Directions are in reverse order until we find the path. *)
        let nextDirections =
            GoOut(targetMetroPlace, connectingExitStreet)
            :: (path |> List.map TakeMetro)
            @ directions

        directionsToNode'
            nextDirections
            cityId
            targetMetroStation.LeavesToStreet
            destinationNode
    | None -> None

let private bfsMetroStations cityId (currentZone: Zone) (targetZone: Zone) =
    bfsMetroStations' List.empty Set.empty cityId currentZone targetZone

let private bfsMetroStations' path visited cityId currentZone targetZone =
    if Set.contains currentZone.Id visited then
        None
    else if currentZone.Id = targetZone.Id then
        Some path
    else
        let visited = Set.add currentZone.Id visited

        let candidates =
            currentZone.MetroStations
            |> List.ofMap
            |> List.choose (fun (connectingLineId, currentMetroStation) ->
                let connection =
                    Queries.Metro.zoneConnections
                        cityId
                        currentZone
                        connectingLineId

                let bfsOnConnection
                    (line: MetroLine)
                    (connection: MetroStationDestination)
                    =
                    let _, nextZone, _ = connection

                    let nextStation =
                        Queries.Metro.tryStationFromZone'
                            cityId
                            nextZone.Id
                            line.Id
                        |> Option.get (* We've resolved it from the query so it should be safe... Famous words 2.0. *)

                    let path =
                        (currentMetroStation, nextStation, line.Id) :: path

                    bfsMetroStations' path visited cityId nextZone targetZone

                match connection with
                | Some(connections, line) ->
                    match connections with
                    | OnlyNextCoords next -> bfsOnConnection line next
                    | OnlyPreviousCoords prev -> bfsOnConnection line prev
                    | PreviousAndNextCoords(prev, next) ->
                        let prevCandidate = bfsOnConnection line prev
                        let nextCandidate = bfsOnConnection line next

                        [ prevCandidate; nextCandidate ]
                        |> List.choose id
                        |> chooseShortestPath
                | None -> None)

        chooseShortestPath candidates

let private findTargetMetroStation cityId (zone: Zone) =
    Queries.World.placesByTypeInCity cityId PlaceTypeIndex.MetroStation
    |> List.tryFind (fun place -> place.ZoneId = zone.Id)

let private findClosestMetroStation
    cityId
    currentStreetPlace
    destinationNode
    directions
    =
    let zone = Queries.World.zoneInCityById cityId currentStreetPlace.ZoneId
    let metroStationInZone = findTargetMetroStation cityId zone

    match metroStationInZone with
    | Some metroStation ->
        let directionsToMetroStationStreet =
            bfsStreets zone currentStreetPlace.Id metroStation.StreetId

        match directionsToMetroStationStreet with
        | Some [] ->
            (* Metro station is in the same street, so enter it. *)
            let currentStreet =
                Queries.World.streetById cityId currentStreetPlace.Id

            directionsToNode'
                (Enter(currentStreet, metroStation) :: directions)
                cityId
                metroStation.Id
                destinationNode
        | Some directionsToMetroStationStreet ->
            (* Metro station is in a different street, enter it after walking there. *)
            let _, _, nextStreetId = List.head directionsToMetroStationStreet
            let nextStreet = Queries.World.streetById cityId nextStreetId

            let directionsToMetroStationStreet =
                directionsToMetroStationStreet |> mapToWalkableDirections cityId

            directionsToNode'
                (Enter(nextStreet, metroStation)
                 :: directionsToMetroStationStreet
                 @ directions)
                cityId
                metroStation.Id
                destinationNode
        | None -> None
    | None -> None

let private findConnectionInSameZone
    cityId
    currentPlace
    targetPlace
    destinationNode
    directions
    =
    let zone = Queries.World.zoneInCityById cityId currentPlace.ZoneId

    match bfsStreets zone currentPlace.Id targetPlace.StreetId with
    | Some directionsToNextStreet ->
        let _, _, nextStreetId = List.head directionsToNextStreet

        let directionsToNextStreet =
            directionsToNextStreet |> mapToWalkableDirections cityId

        directionsToNode'
            (directionsToNextStreet @ directions)
            cityId
            nextStreetId
            destinationNode
    | None -> None // This should not happen.

let private mapToWalkableDirections cityId =
    List.map (fun (direction, fromStreetId, toStreetId) ->
        let fromStreet = Queries.World.streetById cityId fromStreetId

        let toStreet = Queries.World.streetById cityId toStreetId

        Walk(fromStreet, toStreet, direction))

let private bfsStreets zone currentStreetId targetStreetId =
    bfsStreets' List.empty Set.empty zone currentStreetId targetStreetId

let private bfsStreets'
    path
    visited
    zone
    currentStreetId
    targetStreetId
    : (Direction * NodeId * NodeId) list option =
    if Set.contains currentStreetId visited then
        None
    else if currentStreetId = targetStreetId then
        Some path
    else
        let connections =
            zone.Streets.Connections
            |> Map.tryFind currentStreetId
            |> Option.defaultValue Map.empty
            |> Map.toList

        match connections with
        | [] -> None
        | connections ->
            let visited = Set.add currentStreetId visited

            let candidates =
                connections
                |> List.choose (fun (direction, nextId) ->
                    let path = (direction, currentStreetId, nextId) :: path

                    bfsStreets' path visited zone nextId targetStreetId)

            match candidates with
            | [] -> None
            | _ -> chooseShortestPath candidates

let private chooseShortestPath<'a> (candidates: 'a list list) : 'a list option =
    match candidates with
    | [] -> None
    | _ ->
        (List.head candidates, candidates)
        ||> List.fold (fun shortestPath candidate ->
            let shortestLength = List.length shortestPath
            let candidateLength = List.length candidate

            if candidateLength < shortestLength then
                candidate
            else
                shortestPath)
        |> Some
