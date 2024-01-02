namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module OrderMerchandiseCommand =
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

                let merchItem =
                    showCancellableChoicePrompt
                        $"""What kind of {"merchandise" |> Styles.prompt} would you like to order?"""
                        Generic.cancel
                        (fun
                            (item: Item, minQuantity, maxQuantity, pricePerItem) ->
                            $"""{item.Name |> Styles.item}, {pricePerItem |> Styles.money} / item.{Styles.Spacing.choicePromptNewLine}At least {minQuantity}, at most {maxQuantity}.""")
                        availableItems

                Scene.World }
