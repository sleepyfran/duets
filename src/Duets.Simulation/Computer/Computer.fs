module Duets.Simulation.Computer

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let private updateComputer fn item computer =
    let updatedComputer = fn computer

    let updatedItem =
        Item.updateProperty (Usable(Computer updatedComputer)) item

    [ Diff(item, updatedItem) |> ItemChangedInCharacterInventory
      Situations.focused (UsingComputer(updatedItem, updatedComputer)) ]

/// Opens an app on the given computer.
let openApp app =
    updateComputer (fun computer ->
        { computer with
            ComputerState = AppRunning app })

/// Closes the currently running app on the given computer.
let closeApp =
    updateComputer (fun computer ->
        { computer with
            ComputerState = AppSwitcher })
