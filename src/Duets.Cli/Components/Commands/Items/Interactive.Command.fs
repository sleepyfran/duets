namespace Duets.Cli.Components.Commands

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

[<RequireQualifiedAccess>]
module InteractiveCommand =
    let exercise =
        Command.itemInteraction
            (Command.VerbWithPrepositions("exercise", [ "with" ]))
            Command.exerciseDescription
            (ItemInteraction.Interactive InteractiveItemInteraction.Exercise)
            (function
             | Ok effects ->
                 (Interaction.exerciseSteps, 2<second>) ||> showProgressBarAsync
                 Interaction.exerciseResult |> showMessage
                 effects |> Duets.Cli.Effect.applyMultiple
                 Scene.World
             | Error _ ->
                 Items.itemCannotBeExercisedWith |> showMessage
                 Scene.World)

    let play =
        Command.itemInteraction
            (Command.VerbOnly "play")
            Command.playGameDescription
            (ItemInteraction.Interactive InteractiveItemInteraction.Play)
            (function
             | Ok effects ->
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
