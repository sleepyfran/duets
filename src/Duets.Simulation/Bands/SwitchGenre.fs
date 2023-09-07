module Duets.Simulation.Bands.SwitchGenre

open Duets.Entities
open Duets.Simulation

/// Switches the currently selected band genre if the updated genre is different
/// from the current one, otherwise returns None.
let switchGenre state updatedGenre =
    let currentBand = Queries.Bands.currentBand state

    if currentBand.Genre <> updatedGenre then
        BandSwitchedGenre(currentBand, Diff(currentBand.Genre, updatedGenre))
        |> Some
    else
        None
