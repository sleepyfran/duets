namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation

[<RequireQualifiedAccess>]
module OrderCommand =
    let private orderInteractive availableItems =
        let selectedItem =
            showOptionalChoicePrompt
                Shop.itemPrompt
                Generic.nothing
                Shop.itemInteractiveRow
                availableItems

        match selectedItem with
        | Some item ->
            let orderResult =
                Shop.order (State.get ()) item

            match orderResult with
            | Ok effects -> Cli.Effect.applyMultiple effects
            | Error _ -> Shop.notEnoughFunds |> showMessage
        | None -> ()

    let private orderFromArgs input availableItems =
        let orderResult =
            Shop.orderByName (State.get ()) input availableItems

        match orderResult with
        | Ok effects -> Cli.Effect.applyMultiple effects
        | Error Shop.ItemNotFound -> Shop.itemNotFound input |> showMessage
        | Error Shop.NotEnoughFunds -> Shop.notEnoughFunds |> showMessage

    /// Command to order something from a bar by specifying the name of the item
    /// via the command arguments.
    let create availableItems =
        { Name = "order"
          Description = Command.orderDescription
          Handler =
            (fun args ->
                let input = args |> String.concat " "

                if String.isEmpty input then
                    orderInteractive availableItems
                else
                    orderFromArgs input availableItems

                Scene.World) }
