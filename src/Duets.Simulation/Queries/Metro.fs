namespace Duets.Simulation.Queries

open Duets.Common
open Duets.Entities

module Metro =
    /// Attempts to find a station that belongs to the given metro line in the
    /// given zone. If the station does not belong to the line, it returns None.
    let tryStationFromZone state zoneId lineId =
        let zone = World.zoneInCurrentCityById state zoneId
        zone.MetroStations |> Map.tryFind lineId

    /// Attempts to find the current station that the character is in.
    let tryCurrentStation state =
        let zone, street = World.currentZoneCoordinates state

        zone.MetroStations
        |> List.ofMapValues
        |> List.tryFind (fun station -> station.LeavesToStreet = street.Id)

    /// Attempts to find the station line of the given station. If the station
    /// does not belong to the metro line of the current city, it returns None.
    let tryStationLine state station =
        let currentCity = World.currentCity state

        currentCity.MetroLines |> Map.tryFind station.Line

    /// Returns the current station line. If the character is not in a metro
    /// station, it returns None.
    let tryCurrentStationLine state =
        tryCurrentStation state |> Option.bind (tryStationLine state)

    /// Returns the connections of a given station in the current metro line.
    let stationConnections state currentStation =
        let zone, _ = World.currentZoneCoordinates state
        let metroLine = tryCurrentStationLine state

        let resolveConnectionToZone targedZoneId =
            let station =
                tryStationFromZone state targedZoneId currentStation.Line
                |> Option.get

            let resolvedZone = World.zoneInCurrentCityById state targedZoneId

            let resolvedPlace =
                World.placeInCurrentCityById state station.PlaceId

            let targetCoords =
                targedZoneId, station.LeavesToStreet, station.PlaceId

            resolvedPlace, resolvedZone, targetCoords

        metroLine
        |> Option.bind (fun line -> Map.tryFind zone.Id line.Stations)
        |> Option.map (fun connections ->
            match connections with
            | OnlyNext nextZoneId ->
                resolveConnectionToZone nextZoneId |> OnlyNextCoords
            | OnlyPrevious previousZoneId ->
                resolveConnectionToZone previousZoneId |> OnlyPreviousCoords
            | PreviousAndNext(previousZoneId, nextZoneId) ->
                let previous = resolveConnectionToZone previousZoneId
                let next = resolveConnectionToZone nextZoneId

                PreviousAndNextCoords(previous, next))

    /// Returns the time that needs to pass for another train to overlap with
    /// the current turn.
    let timeToOverlapWithTrain state stationLine =
        let turnInfo = Calendar.currentTurnInformation state
        let currentTurnMinutes = turnInfo.TimeSpent

        stationLine.UsualWaitingTime
        - (currentTurnMinutes % stationLine.UsualWaitingTime)

    /// Returns whether the time that has passed so far in the turn overlaps
    /// with the usual waiting time of the given station line. To simplify
    /// things the game assumes that every metro departs every N minutes since
    /// the minute 0 of the turn, where N is the usual waiting time of the line.
    let timeOverlapsWithWaitingTime state stationLine =
        let turnInfo = Calendar.currentTurnInformation state
        let currentTurnMinutes = turnInfo.TimeSpent

        currentTurnMinutes % stationLine.UsualWaitingTime = 0<minute>
