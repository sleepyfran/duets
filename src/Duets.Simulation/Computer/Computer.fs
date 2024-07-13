module Duets.Simulation.Computer

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

/// Opens an app on the given computer.
let openApp item computer app =
    let updatedComputer =
        { computer with
            ComputerState = AppRunning app }

    let updatedItem =
        Item.updateProperty (Usable(Computer updatedComputer)) item

    [ Diff(item, updatedItem) |> ItemChangedInCharacterInventory
      Situations.focused (UsingComputer(updatedItem, updatedComputer)) ]
