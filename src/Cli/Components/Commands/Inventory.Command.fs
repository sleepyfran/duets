namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

[<RequireQualifiedAccess>]
module InventoryCommand =
    /// Command which displays what the character is currently carrying in their
    /// inventory.
    let create inventory =
        { Name = "inventory"
          Description = Command.inventoryDescription
          Handler =
            fun _ ->
                if List.isEmpty inventory then
                    Inventory.noItems |> showMessage
                else
                    Inventory.itemsCurrentlyCarrying |> showMessage

                    inventory
                    |> List.map Inventory.itemRow
                    |> List.iter showMessage

                Scene.World }
