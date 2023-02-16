namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

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
                 effects |> Duets.Cli.Effect.applyMultiple
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
                 effects |> Duets.Cli.Effect.applyMultiple
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
                 effects |> Duets.Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemCannotBeWatched |> showMessage
                 Scene.World)
