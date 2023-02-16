namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

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
