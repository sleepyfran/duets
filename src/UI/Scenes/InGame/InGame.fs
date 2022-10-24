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
    Run.create [ Run.text "" ]

let private categorizeInteractions interactions =
    let freeRoamInteractions, restOfInteractions =
        interactions
        |> List.partition (fun item ->
            match item.Interaction with
            | Interaction.FreeRoam _ -> true
            | _ -> false)

    (* Order them so that the movement and free-roam interactions will always be in the end. *)
    [
        restOfInteractions
        freeRoamInteractions
    ]

let private runInteraction state interaction =
    match interaction.Interaction with
    | Interaction.FreeRoam fr -> FreeRoam.runFreeRoamInteraction state fr
    | _ -> failwith "Oops. Still not quite there yet!"

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
        |> categorizeInteractions

    let message =
        [ textFromCoordinates currentPlace ]
        |> createMessage

    let choices =
        Choice.create
            {
                Id = placeId.ToString()
                OnSelected = (handleInteraction state viewStack)
                ToText = (Text.World.Interactions.get state.Current)
                Values = Choice.Sectioned interactions
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