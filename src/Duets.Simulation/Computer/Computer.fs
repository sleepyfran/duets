module Duets.Simulation.Computer

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let private updateItem fn item computer =
    let updatedComputer = fn computer

    let updatedItem =
        Item.updateProperty (Usable(Computer updatedComputer)) item

    updatedComputer, updatedItem

let private updateComputer fn item computer =
    let updatedComputer, updatedItem = updateItem fn item computer

    [ Diff(item, updatedItem) |> ItemChangedInCharacterInventory ]

let private updateComputerInUse fn item computer =
    let updatedComputer, updatedItem = updateItem fn item computer

    [ Diff(item, updatedItem) |> ItemChangedInCharacterInventory
      Situations.focused (UsingComputer(updatedItem, updatedComputer)) ]


/// Opens an app on the given computer.
let openApp app =
    updateComputerInUse (fun computer ->
        { computer with
            ComputerState = AppRunning app })

/// Closes the currently running app on the given computer.
let closeApp =
    updateComputerInUse (fun computer ->
        { computer with
            ComputerState = AppSwitcher })

/// Turns off the given computer and sets the Situation back to free roaming.
let turnOff item computer =
    let computerEffects =
        updateComputer
            (fun computer ->
                { computer with
                    ComputerState = Booting })
            item
            computer

    computerEffects @ [ Situations.freeRoam ]
