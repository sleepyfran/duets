namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module CookCommand =
    /// Command to cook food.
    let create availableItems =
        { Name = "cook"
          Description = "Allows you to cook a recipe"
          Handler =
            (fun _ ->
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

                    showProgressBarSync
                        [ "Chopping stuff..." |> Styles.progress
                          "Mixing..." |> Styles.progress
                          "Cooking..." |> Styles.progress ]
                        1<second>

                    match orderResult with
                    | Ok effects ->
                        "You cooked a delicious meal!"
                        |> Styles.success
                        |> showMessage

                        Duets.Cli.Effect.applyMultiple effects
                    | Error _ ->
                        "You don't have enough money to buy the ingredients for that"
                        |> Styles.error
                        |> showMessage
                | None -> ()

                Scene.World) }
