namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Interactions

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
                 effects |> Cli.Effect.applyMultiple
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
                 effects |> Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemNotEdible |> showMessage
                 Scene.World)
