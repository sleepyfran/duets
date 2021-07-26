module Ui.Components.Navigation

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Elmish
open Ui.Types

type Msg =
    | GoBack
    | Exit

let update msg state =
    match msg with
    | GoBack ->
        match state with
        | PreGameState preGameState ->
            (PreGameState
                { preGameState with
                      NavigationStack = List.skip 1 preGameState.NavigationStack },
             Cmd.none)
        | GameState gameState -> (GameState gameState, Cmd.none)
    | Exit ->
        Environment.Exit(0)
        (state, Cmd.none)

let private canGoBack state =
    match state with
    | PreGameState preGameState -> List.length preGameState.NavigationStack > 1
    | GameState _ -> false

let view state dispatch =
    if canGoBack state then
        Button.create [
            Button.content "◀️️ Go back"
            Button.onClick (fun _ -> dispatch GoBack)
        ]
    else
        Button.create [
            Button.content "❌ Exit"
            Button.onClick (fun _ -> dispatch Exit)
        ]
