namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module OrderCommand =
    /// Command to order something from a shop by specifying the name of the item
    /// via the command arguments or selecting it interactively.
    let create availableItems =
        { Name = "order"
          Description = Command.orderDescription
          Handler =
            fun args ->
                let toReferenceName (item: Item, _) = item.Name

                let item =
                    Selection.fromArgsOrInteractive
                        args
                        Shop.itemPrompt
                        availableItems
                        Shop.itemInteractiveRow
                        toReferenceName

                match item with
                | Selection.Selected item ->
                    let orderResult = Shop.order (State.get ()) item

                    match orderResult with
                    | Ok effects -> Duets.Cli.Effect.applyMultiple effects
                    | Error _ -> Shop.notEnoughFunds |> showMessage
                | Selection.NoMatchingItem input ->
                    Shop.itemNotFound input |> showMessage
                | Selection.Cancelled -> ()

                Scene.World }
