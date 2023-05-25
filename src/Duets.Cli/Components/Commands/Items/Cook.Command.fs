namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module CookCommand =
    let private notEnoughFundsError =
        "You don't have enough money to buy the ingredients for that"
        |> Styles.error

    let private showCookingResult effects =
        showProgressBarSync
            [ "Chopping stuff..." |> Styles.progress
              "Mixing..." |> Styles.progress
              "Cooking..." |> Styles.progress ]
            1<second>

        "You cooked a delicious meal!" |> Styles.success |> showMessage
        Duets.Cli.Effect.applyMultiple effects

    let private cookInteractive availableItems =
        let orderedItems =
            availableItems |> Seq.sortBy (fun (item, _) -> item.Brand)

        let selectedItem =
            showOptionalChoicePrompt
                "What do you want to cook?"
                Generic.nothing
                (fun (item, price) ->
                    $"{item.Brand} ({Styles.money price} for ingredients)")
                orderedItems

        match selectedItem with
        | Some item ->
            let orderResult = Shop.order (State.get ()) item

            match orderResult with
            | Ok effects -> showCookingResult effects
            | Error _ -> notEnoughFundsError |> showMessage
        | None -> ()

    let private cookFromArgs input availableItems =
        let orderResult = Shop.orderByName (State.get ()) input availableItems

        match orderResult with
        | Ok effects -> showCookingResult effects
        | Error Shop.ItemNotFound ->
            $"There's no such recipe as {input}" |> Styles.error |> showMessage
        | Error Shop.NotEnoughFunds -> notEnoughFundsError |> showMessage

    /// Command to cook food.
    let create availableItems =
        { Name = "cook"
          Description = "Allows you to cook a recipe"
          Handler =
            (fun args ->
                let input = args |> String.concat " "

                if String.isEmpty input then
                    cookInteractive availableItems
                else
                    cookFromArgs input availableItems

                Scene.World) }
