module rec Duets.UI.Hooks.Effect

open Duets.Agents
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.UI.Components.Run
open Duets.UI.Hooks.ViewStack
open Duets.UI.Types

let private runEffect viewStack effect =
    let effects, state = Simulation.tickOne (State.get ()) effect

    State.set state

    effects |> Seq.tap Log.appendEffect |> Seq.iter (displayEffect viewStack)

let private displayEffect viewStack effect =
    match effect with
    | SongStarted (_, Unfinished(song, _, _)) ->
        [
            success
                $"""Your band has started working on a {song.VocalStyle} song called "{song.Name}". You can cancel, finish or improve it in the rehearsal room"""
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongImproved (_, Diff(Unfinished(_, _, previousQuality), Unfinished(_, _, currentQuality))) ->
        [
            createText
                $"You've improved the song. It improved from {previousQuality} to {currentQuality}"
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongPracticed (_, Finished(song, _)) ->
        [
            createText
                $"Your band improved its practice of {song.Name} to {song.Practice}%%"
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongDiscarded (_, Unfinished(song, _, _)) ->
        [
            createText $"Your band decided to stop working on {song.Name}"
        ]
        |> InGameAction.Message
        |> viewStack.AddAction
    | SongFinished (_, Finished(song, quality), _) ->
        [
            createText
                $"""Your band finished the song "{song.Name}". The result quality is {quality}"""
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
