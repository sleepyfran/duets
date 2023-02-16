namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Interactions

[<RequireQualifiedAccess>]
module ConsumeCommands =
    /// Command that attempts to drink an item given its name.
    let drink =
        Command.itemInteraction
            (Command.VerbOnly "drink")
            Command.drinkDescription
            (ItemInteraction.Consumable ConsumableItemInteraction.Drink)
            (function
             | Ok effects ->
                 Items.drunkItem |> showMessage
                 wait 1000<millisecond>
                 effects |> Duets.Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemNotDrinkable |> showMessage
                 Scene.World)

    /// Command that attempts to eat an item given its name.
    let eat =
        Command.itemInteraction
            (Command.VerbOnly "eat")
            Command.eatDescription
            (ItemInteraction.Consumable ConsumableItemInteraction.Eat)
            (function
             | Ok effects ->
                 Items.ateItem |> showMessage
                 wait 1000<millisecond>
                 effects |> Duets.Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemNotEdible |> showMessage
                 Scene.World)
