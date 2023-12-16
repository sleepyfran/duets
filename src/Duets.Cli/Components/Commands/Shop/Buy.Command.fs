namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation

[<RequireQualifiedAccess>]
module BuyCommand =
    /// Command to buy something from a shop by specifying the name of the item
    /// via the command arguments or selecting it interactively.
    let create availableItems =
        { Name = "buy"
          Description = Command.buyDescription
          Handler =
            fun _ ->
                let selectedItem =
                    showSearchableOptionalChoicePrompt
                        Shop.itemPrompt
                        Generic.cancel
                        Shop.itemInteractiveRow
                        availableItems

                match selectedItem with
                | Some item ->
                    let orderResult = Shop.order (State.get ()) item

                    match orderResult with
                    | Ok effects -> Duets.Cli.Effect.applyMultiple effects
                    | Error _ -> Shop.notEnoughFunds |> showMessage
                | None -> ()

                Scene.World }
