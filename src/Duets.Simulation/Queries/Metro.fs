namespace Duets.Simulation.Queries

open Duets.Entities

module Metro =
    /// Returns the current station line. If the character is not in a metro
    /// station, it returns None.
    let tryCurrentStationLine state =
        let currentCity = World.currentCity state
        let zone, street = World.currentZoneCoordinates state

        let metroStation =
            zone.MetroStations
            |> List.tryFind (fun station -> station.LeavesToStreet = street.Id)

        metroStation
        |> Option.bind (fun mt -> currentCity.MetroLines |> Map.tryFind mt.Line)

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
