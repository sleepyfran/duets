namespace Simulation.Queries

module Calendar =
    open Aether
    open Entities

    /// Returns the current date in game.
    let today state = state |> Optic.get Lenses.State.today_

    /// Returns the tomorrow date in game.
    let tomorrow state = today state |> Calendar.Ops.addDays 1
