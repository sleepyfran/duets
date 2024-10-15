namespace Duets.Simulation.Queries

module Calendar =
    open Aether
    open Duets.Entities
    open Duets.Simulation

    /// Returns the current date in game.
    let today state = state |> Optic.get Lenses.State.today_

    /// Returns the tomorrow date in game.
    let tomorrow state = today state |> Calendar.Ops.addDays 1

    /// Creates an infinite sequence of dates starting from the current
    /// date in game and adding one day moment for each element.
    let nextDates state =
        let currentDate = today state

        Seq.initInfinite (fun index ->
            currentDate |> Calendar.Query.nextN (index + 1))

    /// Returns information about the current turn, including the current day
    /// moment, the time spent in the current day moment, the duration of the
    /// day moment and the time left in the current day moment.
    let currentTurnInformation state =
        let currentDayMoment = today state |> Calendar.Query.dayMomentOf
        let nextDayMoment = currentDayMoment |> Calendar.Query.nextDayMoment

        let currentTurnTime = state |> Optic.get Lenses.State.turnMinutes_

        let dayMomentDuration = Config.Time.minutesPerDayMoment

        {| CurrentDayMoment = currentDayMoment
           NextDayMoment = nextDayMoment
           TimeSpent = currentTurnTime
           DayMomentDuration = dayMomentDuration
           TimeLeft = dayMomentDuration - currentTurnTime |}
