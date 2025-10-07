namespace Duets.Simulation.Queries

open Duets.Common
open Duets.Entities

module Metro =
    /// Attempts to find a station that belongs to the given metro line in the
    /// given zone of the given city. If the station does not belong to the line,
    /// it returns None.
    let tryStationFromZone' cityId zoneId lineId =
        let zone = World.zoneInCityById cityId zoneId
        zone.MetroStations |> Map.tryFind lineId

    /// Attempts to find a station that belongs to the given metro line in the
    /// given zone. If the station does not belong to the line, it returns None.
    let tryStationFromZone state zoneId lineId =
        let cityId, _, _ = World.currentCoordinates state
        tryStationFromZone' cityId zoneId lineId

    /// Attempts to find the current station that the character is in.
    let tryCurrentStation state =
        let zone, street = World.currentZoneCoordinates state

        zone.MetroStations
        |> List.ofMapValues
        |> List.tryFind (fun station -> station.LeavesToStreet = street.Id)

    /// Returns the lines that belong to the given station.
    let stationLines state station =
        let currentCity = World.currentCity state

        station.Lines |> List.map (fun lineId -> currentCity.MetroLines[lineId])

    /// Returns the current station line. If the character is not in a metro
    /// station, it returns None.
    let currentStationLines state =
        tryCurrentStation state
        |> Option.map (stationLines state)
        |> Option.defaultValue []

    /// Returns the connections of a given zone and line.
    let zoneConnections cityId (zone: Zone) lineId =
        let city = World.cityById cityId
        let metroLine = city.MetroLines |> Map.find lineId

        let resolveConnectionToZone targetZoneId =
            let targetStation =
                tryStationFromZone' cityId targetZoneId lineId |> Option.get

            let resolvedZone = World.zoneInCityById cityId targetZoneId

            let resolvedPlace =
                World.placeInCityById cityId targetStation.PlaceId

            let targetCoords =
                targetZoneId,
                targetStation.LeavesToStreet,
                targetStation.PlaceId

            resolvedPlace, resolvedZone, targetCoords

        let connection =
            Map.tryFind zone.Id metroLine.Stations
            |> Option.map (fun connections ->
                match connections with
                | OnlyNext nextZoneId ->
                    (resolveConnectionToZone nextZoneId) |> OnlyNextCoords
                | OnlyPrevious previousZoneId ->
                    (resolveConnectionToZone previousZoneId)
                    |> OnlyPreviousCoords
                | PreviousAndNext(previousZoneId, nextZoneId) ->
                    let previous = resolveConnectionToZone previousZoneId
                    let next = resolveConnectionToZone nextZoneId

                    PreviousAndNextCoords(previous, next))

        connection |> Option.map (fun connection -> connection, metroLine)

    /// Returns the connections of the current station and line.
    let currentZoneConnections state lineId =
        let city = World.currentCity state
        let zone, _ = World.currentZoneCoordinates state
        zoneConnections city.Id zone lineId

    /// Returns the connections of a given station in the current metro line.
    let stationLineConnections state currentStation =
        currentStation.Lines |> List.choose (currentZoneConnections state)

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
