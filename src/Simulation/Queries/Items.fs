namespace Simulation.Queries

open Aether
open Common
open Entities
open Simulation

module Items =
    /// Returns all the items currently available in the given coordinates.
    let allIn state coords =
        Optic.get Lenses.State.worldItems_ state
        |> Map.tryFind coords
        |> Option.defaultValue []

    /// Returns all the items currently available in the current location.
    let allInCurrentLocation state =
        Queries.World.Common.currentWorldCoordinates state
        |> allIn state

    /// Determines whether the given item is located in the given world location,
    /// the character's inventory or none of them.
    let itemLocation state location item =
        let locationItems = allIn state location
        let inventory = Inventory.get state

        if locationItems |> List.contains item then
            ItemLocation.World
        else if inventory |> List.contains item then
            ItemLocation.Inventory
        else
            ItemLocation.Nowhere
