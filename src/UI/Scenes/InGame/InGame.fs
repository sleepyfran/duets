module UI.Scenes.InGame.Root

open Agents
open Avalonia.Controls
open Avalonia.Controls.Documents
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
open UI.Scenes.InGame.Types

let private textFromCoordinates (place: Place) : IView =
    Span.create [
        Span.inlines [
            match place.Type with
            | RehearsalSpace _ ->
                Run.create [
                    Run.text
                        "You’re in the rehearsal room, the previous band left all their empty beers and a bunch of cigarettes on the floor. Your band’s morale has decreased a bit."
                ]
                :> IView
            | _ -> Run.create []
        ]
    ]

let private filterInteractions interactions =
    interactions
    |> List.filter (fun item ->
        match item.Interaction with
        | Interaction.FreeRoam _ -> false
        | _ -> true)

let private runInteraction state interaction =
    match interaction.Interaction with
    | _ -> Nothing

let private createMessage (content: IView list) : IView =
    RichTextBlock.create [ RichTextBlock.inlines content ]

let private applyEffects state effects =
    effects
    |> List.iter (fun effect ->
        let effects, state =
            Simulation.tick state effect

        State.set state

        effects |> Seq.iter Log.append
    (* TODO: Display effects! *)
    )

let rec handleInteraction
    (state: IReadable<State>)
    (viewStack: IWritable<IView list>)
    interaction
    =
    let action =
        runInteraction state.Current interaction

    match action with
    | Message content ->
        createMessage content :: viewStack.Current
        |> viewStack.Set
    | Subcomponent views -> views @ viewStack.Current |> viewStack.Set
    | Effects effects -> applyEffects state.Current effects
    | Nothing -> ()

let private createInteractionsChoiceView
    (state: IReadable<State>)
    (viewStack: IWritable<IView list>)
    =
    let currentPlace =
        Queries.World.currentPlace state.Current

    let (PlaceId placeId) = currentPlace.Id

    let interactions =
        Queries.Interactions.availableCurrently state.Current
        |> filterInteractions

    let message =
        [ textFromCoordinates currentPlace ]
        |> createMessage

    let choices =
        Choice.create
            {
                Id = placeId.ToString()
                OnSelected = (handleInteraction state viewStack)
                ToText = (Text.World.Interactions.get state.Current)
                Values = interactions
            }

    [ message; choices ]

let view =
    Component.create (
        "InGame",
        fun ctx ->
            let state = ctx.useGameState ()

            let viewStack = ctx.useState<IView list> []

            ctx.useEffect (
                handler =
                    (fun _ ->
                        viewStack.Current
                        @ (createInteractionsChoiceView state viewStack)
                        |> viewStack.Set),
                triggers = [ EffectTrigger.AfterChange state ]
            )

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 15
                StackPanel.children [ yield! viewStack.Current ]
            ]
    )
