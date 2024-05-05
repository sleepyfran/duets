module Duets.Simulation.Bands.SwitchGenre

open Duets.Entities
open Duets.Simulation

/// Switches the currently selected band genre if the updated genre is different
/// from the current one, otherwise returns a band already has genre error.
let switchGenre state band updatedGenre =
    if band.Genre <> updatedGenre then
        BandSwitchedGenre(band, Diff(band.Genre, updatedGenre))
        |> List.singleton
        |> Ok
    else
        BandAlreadyHasGenre updatedGenre |> Error
