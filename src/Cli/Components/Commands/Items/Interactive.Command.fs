namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

[<RequireQualifiedAccess>]
module InteractiveCommand =
    let sleep =
        Command.itemInteraction
            (Command.VerbWithPrepositions("sleep", [ "in"; "on" ]))
            Command.sleepDescription
            (ItemInteraction.Interactive InteractiveItemInteraction.Sleep)
            (function
             | Ok effects ->
                 Interaction.sleeping |> showMessage
                 wait 8000<millisecond>
                 Interaction.sleepResult |> showMessage
                 effects |> Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemCannotBeUsedForSleeping |> showMessage
                 Scene.World)

    let play =
        Command.itemInteraction
            (Command.VerbOnly "play")
            Command.playConsoleDescription
            (ItemInteraction.Interactive InteractiveItemInteraction.Play)
            (function
             | Ok effects ->
                 wait 1000<millisecond>
                 Interaction.playConsoleResult |> showMessage
                 effects |> Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemCannotBePlayedWith |> showMessage
                 Scene.World)

    let watch =
        Command.itemInteraction
            (Command.VerbOnly "watch")
            Command.watchTvDescription
            (ItemInteraction.Interactive InteractiveItemInteraction.Watch)
            (function
             | Ok effects ->
                 wait 1000<millisecond>
                 Interaction.watchTvResult |> showMessage
                 effects |> Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemCannotBeWatched |> showMessage
                 Scene.World)
