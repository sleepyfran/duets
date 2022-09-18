namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text

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
                    Items.noItemsInventory |> showMessage
                else
                    Items.itemsCurrentlyCarrying |> showMessage

                    inventory
                    |> List.map Items.itemRow
                    |> List.iter showMessage

                Scene.World }
