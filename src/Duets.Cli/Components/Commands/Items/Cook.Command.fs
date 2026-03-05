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
    type private CookChoice =
        | Recipe of Item * decimal<dd>
        | More

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

    let private doCook itemWithPrice =
        match Cooking.cook (State.get ()) itemWithPrice with
        | Ok effects -> showCookingResult effects
        | Error _ -> notEnoughFundsError |> showMessage

    let private toItemStr (item: Item, price: decimal<dd>) =
        $"{Generic.itemDetailedName item} ({Styles.money price} for ingredients)"

    /// Command to cook food.
    let create availableItems =
        { Name = "cook"
          Description = "Allows you to cook a recipe"
          Handler =
            (fun args ->
                let homeMeals = Cooking.homeMeals
                let input = args |> String.concat " "

                if String.isEmpty input then
                    let options =
                        (homeMeals |> List.map (fun (i, p) -> Recipe(i, p)))
                        @ [ More ]

                    let toChoiceStr =
                        function
                        | Recipe(item, price) -> toItemStr (item, price)
                        | More -> Styles.faded "More recipes..."

                    match showSearchableOptionalChoicePrompt
                              "What do you want to cook?"
                              Generic.nothing
                              toChoiceStr
                              options with
                    | Some(Recipe(item, price)) -> doCook (item, price)
                    | Some More ->
                        match showSearchableOptionalChoicePrompt
                                  "What other recipe do you want to cook?"
                                  Generic.nothing
                                  toItemStr
                                  availableItems with
                        | Some item -> doCook item
                        | None -> ()
                    | None -> ()
                else
                    let allItems =
                        homeMeals @ availableItems
                        |> List.distinctBy (fun (item: Item, _) -> item.Name)

                    let found =
                        allItems
                        |> List.tryFind (fun (item: Item, _) ->
                            String.diacriticInsensitiveContains item.Name input)

                    match found with
                    | Some item -> doCook item
                    | None ->
                        $"There's no such recipe as {input}"
                        |> Styles.error
                        |> showMessage

                Scene.World) }
