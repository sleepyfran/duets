namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Merchandise.Order

[<RequireQualifiedAccess>]
module OrderMerchandiseCommand =
    let private promptForQuantity merchItem =
        showRangedNumberPrompt
            merchItem.MinPieces
            merchItem.MaxPieces
            $"""How {"many" |> Styles.prompt} do you want to order?"""

    let private showResult res =
        match res with
        | Ok effects ->
            effects |> Effect.applyMultiple

            "The merchant says:" |> showMessage

            "Thank you! Your order will be ready for pick-up here in a week."
            |> Styles.dialog
            |> showMessage
        | Error(MinNotReached min) ->
            $"You need to order at least {min} pieces for the order to go in."
            |> Styles.error
            |> showMessage
        | Error(MaxSurpassed max) ->
            $"You can't order more than {max} pieces."
            |> Styles.error
            |> showMessage
        | Error NotEnoughFunds ->
            $"You don't have enough money on your band's bank account to afford that."
            |> Styles.error
            |> showMessage

    /// Command to order merchandise from a merch workshop.
    let create (availableItems: MerchandiseItem list) =
        { Name = "order"
          Description = "Allows you to order merchandise from the current shop"
          Handler =
            fun _ ->
                "The merchant says:" |> showMessage

                "All items have a 50% discount when ordering more than 100 items"
                |> Styles.dialog
                |> showMessage

                "Orders will be delivered in a week after the order is placed."
                |> Styles.dialog
                |> showMessage

                let merchItem =
                    showCancellableChoicePrompt
                        $"""What kind of {"merchandise" |> Styles.prompt} would you like to order?"""
                        Generic.cancel
                        (fun merchItem ->
                            $"""{merchItem.Item.Name |> Styles.item}, {merchItem.PricePerPiece |> Styles.money} / item.{Styles.Spacing.choicePromptNewLine}At least {merchItem.MinPieces}, at most {merchItem.MaxPieces}.""")
                        availableItems

                match merchItem with
                | Some item ->
                    promptForQuantity item
                    |> orderMerch (State.get ()) item
                    |> showResult
                | None -> ()

                Scene.World }
