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

    /// Command to cook food.
    let create availableItems =
        { Name = "cook"
          Description = "Allows you to cook a recipe"
          Handler =
            (fun args ->
                let toString (item, price) =
                    $"{Generic.itemNameWithDetail item} ({Styles.money price} for ingredients)"

                let toReferenceName (item, _) = item.Brand
                
                let recipe =
                    Selection.fromArgsOrInteractive
                        args
                        "What do you want to cook?"
                        availableItems
                        toString
                        toReferenceName

                match recipe with
                | Selection.Selected item ->
                    let orderResult = Shop.order (State.get ()) item

                    match orderResult with
                    | Ok effects -> showCookingResult effects
                    | Error _ -> notEnoughFundsError |> showMessage
                | Selection.NoMatchingItem input ->
                    $"There's no such recipe as {input}"
                    |> Styles.error
                    |> showMessage
                | Selection.Cancelled -> ()

                Scene.World) }
