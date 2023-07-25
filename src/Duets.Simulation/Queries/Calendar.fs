namespace Duets.Simulation.Queries

module Calendar =
    open Aether
    open Duets.Entities

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
