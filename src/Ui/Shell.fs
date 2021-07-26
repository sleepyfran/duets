namespace Ui

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Elmish

open Entities
open Savegame
open Ui

/// This is the main view of the UI in which we handle the upper level views
/// as well as the global UI state.
module Shell =
    type Msg =
        | StartScreenMsg of Screens.StartScreen.Msg
        | CreatorScreenMsg of Screens.Creator.Msg
        | Effect of Effect

    let init () =
        (PreGameState
            { Screen = PreGameScreen.Start
              Savegame = NotAvailable },
         Cmd.none)

    /// Calls the given update function (partially applied with the message)
    /// only if the state is in PreGame state.
    let private preGameUpdate state updateFn msgType =
        match state with
        | PreGameState preGameState ->
            let (updatedState, cmd) = updateFn preGameState

            (PreGameState updatedState, Cmd.map msgType cmd)
        | _ -> (state, Cmd.none)

    let update msg state : UiState * Cmd<_> =
        match msg with
        | StartScreenMsg startScreenMsg ->
            preGameUpdate
                state
                (Screens.StartScreen.update startScreenMsg)
                StartScreenMsg
        | CreatorScreenMsg creatorScreenMsg ->
            preGameUpdate
                state
                (Screens.Creator.update creatorScreenMsg)
                CreatorScreenMsg
        | Effect effect ->
            match state with
            | PreGameState _ -> (state, Cmd.none)
            | GameState gameState ->
                (GameState
                    { gameState with
                          State = State.Root.apply effect gameState.State },
                 Cmd.none)

    let view state dispatch =
        StackPanel.create [
            StackPanel.margin (30.0, 30.0)
            StackPanel.children [
                match state with
                | PreGameState state ->
                    match state.Screen with
                    | Start ->
                        Screens.StartScreen.view
                            state
                            (StartScreenMsg >> dispatch)
                    | Creator ->
                        Screens.Creator.view
                            state
                            (CreatorScreenMsg >> dispatch)
                | GameState _ ->
                    TextBlock.create [
                        TextBlock.text "Coming soon"
                        TextBlock.fontSize 48.0
                    ]
            ]
        ]

    /// Entry point to the application, which loads Elmish and runs the main
    /// view defined above.
    type MainControl() as this =
        inherit HostControl()

        do
            Elmish.Program.mkProgram init update view
            |> Program.withHost this
            |> Program.run
