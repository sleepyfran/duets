module rec UI.Hooks.Effect

open Agents
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Common
open Entities
open Simulation
open UI.Hooks.ViewStack
open UI.Types

let private runEffect viewStack effect =
    let effects, state = Simulation.tick (State.get ()) effect

    State.set state

    effects |> Seq.tap Log.append |> Seq.iter (displayEffect viewStack)

let private displayEffect viewStack effect =
    match effect with
    | SongStarted (_, (UnfinishedSong song, _, _)) ->
        [
            Run.createText
                $"""Your band has started working on the song "{song.Name}". You can finish or improve it through the compose section in the rehearsal room"""
            :> IView
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongImproved (_, Diff (before, after)) ->
        let (_, _, previousQuality) = before
        let (_, _, currentQuality) = after

        [
            Run.createText
                $"You've improved the song. It improved from {previousQuality} to {currentQuality}"
            :> IView
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongPracticed (_, (FinishedSong song, _)) ->
        [
            Run.createText
                $"Your band improved its practice of {song.Name} to {song.Practice}%%"
            :> IView
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongDiscarded (_, (UnfinishedSong song, _, _)) ->
        [
            Run.createText $"Your band decided to stop working on {song.Name}"
            :> IView
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongFinished (_, (FinishedSong song, quality)) ->
        [
            Run.createText
                $"""Your band finished the song "{song.Name}". The result quality is {quality}"""
            :> IView
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | _ -> ()

type IComponentContext with

    /// Provides a function that can be used to execute an effect, which runs
    /// the effect through the Simulation and shows all the effects that happened
    /// during the simulation, logs them and saves the game.
    member this.useEffectRunner() : Effect list -> unit =
        let viewStack = this.useViewStack ()
        List.iter (runEffect viewStack)
