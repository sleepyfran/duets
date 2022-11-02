module UI.Scenes.InGame.Root

open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Common
open Entities
open Simulation
open UI
open UI.Components
open UI.Hooks.GameState
open UI.Hooks.ViewStack
open UI.Types

let private filterInteractions interactions =
    interactions
    |> List.filter (fun item ->
        match item.Interaction with
        | Interaction.FreeRoam _ -> false
        | _ -> true)

let private runInteraction _ interaction =
    match interaction.Interaction with
    | Interaction.Rehearsal rehearsalInteraction ->
        match rehearsalInteraction with
        | RehearsalInteraction.ComposeNewSong ->
            Subcomponent [
                Interactions.RehearsalRoom.ComposeSong.view
            ]
        | _ -> Nothing
    | _ -> Nothing

let rec handleInteraction
    (state: IReadable<State>)
    (viewStack: ViewStack)
    interaction
    =
    let action =
        runInteraction state.Current interaction

    viewStack.AddAction action

let private addInteractionsChoiceView
    (state: IReadable<State>)
    (viewStack: ViewStack)
    =
    let currentPlace =
        Queries.World.currentPlace state.Current

    let (PlaceId placeId) = currentPlace.Id

    let interactions =
        Queries.Interactions.availableCurrently state.Current
        |> filterInteractions

    [ Text.World.Places.text currentPlace ]
    |> Message
    |> viewStack.AddAction

    [
        Choice.create
            {
                Id = placeId.ToString()
                OnSelected = (handleInteraction state viewStack)
                ToText = (Text.World.Interactions.get state.Current)
                Values = interactions
            }
        :> IView
    ]
    |> Subcomponent
    |> viewStack.AddAction

let view =
    Component.create (
        "InGame",
        fun ctx ->
            let state = ctx.useGameState ()
            let viewStack = ctx.useViewStack ()

            let currentScene =
                ctx.usePassedRead Store.shared.CurrentScene

            ctx.useEffect (
                handler = (fun _ -> addInteractionsChoiceView state viewStack),
                triggers = [
                    EffectTrigger.AfterInit
                    EffectTrigger.AfterChange currentScene
                ]
            )

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 15
                StackPanel.children [ yield! viewStack.Stack.Current ]
            ]
    )
