namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module ShoppingCommand =
    let private handler availableItems args =
        let toReferenceName (item, _) = item.Brand

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

        Scene.World

    /// Command to order something from a shop by specifying the name of the item
    /// via the command arguments or selecting it interactively.
    let createOrder availableItems =
        { Name = "order"
          Description = Command.orderDescription
          Handler = handler availableItems }

    /// Command to buy something from a shop by specifying the name of the item
    /// via the command arguments or selecting it interactively.
    let createBuy availableItems =
        { Name = "buy"
          Description = Command.buyDescription
          Handler = handler availableItems }
