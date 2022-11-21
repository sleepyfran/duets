module UI.Scenes.InGame.Root

open Agents
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
            Subcomponent [ RehearsalRoom.ComposeSong.view ]
        | _ -> Nothing
    | _ -> Nothing

let rec handleInteraction state (viewStack: ViewStack) interaction =
    let action = runInteraction state interaction

    viewStack.AddAction action

let private addInteractionsChoiceView (viewStack: ViewStack) =
    let state = State.get ()

    let currentPlace = Queries.World.currentPlace state

    let (PlaceId placeId) = currentPlace.Id

    let interactions =
        Queries.Interactions.availableCurrently state |> filterInteractions

    [ Text.World.Places.text currentPlace ] |> Message |> viewStack.AddAction

    [
        Choice.create
            {
                Id = placeId.ToString()
                OnSelected = (handleInteraction state viewStack)
                ChoiceContent =
                    (fun item -> {
                        Text = Text.World.Interactions.get state item
                        Classes = []
                        Enabled = true
                    })
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
            let viewStack = ctx.useViewStack ()

            let currentScene = ctx.usePassedRead Store.shared.CurrentScene

            ctx.useEffect (
                handler = (fun _ -> addInteractionsChoiceView viewStack),
                triggers = [ EffectTrigger.AfterInit ]
            )

            ctx.useEffect (
                handler =
                    (fun _ ->
                        [ Divider.vertical :> IView ]
                        |> InGameAction.Subcomponent
                        |> viewStack.AddAction

                        addInteractionsChoiceView viewStack),
                triggers = [ EffectTrigger.AfterChange currentScene ]
            )

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 15
                StackPanel.children [ yield! viewStack.Stack.Current ]
            ]
    )
