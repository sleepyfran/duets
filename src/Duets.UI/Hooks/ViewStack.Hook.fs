module Duets.UI.Hooks.ViewStack

open Duets.Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Duets.Simulation
open Duets.UI
open Duets.UI.Types

type ViewStack = {
    Stack: IReadable<IView list>
    AddAction: InGameAction -> unit
}

let private createMessage (content: IView list) : IView =
    StackPanel.create [
        StackPanel.orientation Orientation.Horizontal
        StackPanel.spacing 5
        StackPanel.children content
    ]
    :> IView

let private applyEffects effects =
    effects
    |> List.iter (fun effect ->
        let effects, state =
            Simulation.tickOne (State.get ()) effect

        State.set state

        effects |> Seq.iter Log.appendEffect
    (* TODO: Display effects! *)
    )

type IComponentContext with
    /// Provides a function that can add actions to the view stack, which
    /// accumulates views to show in the in-game scene.
    member this.useViewStack() : ViewStack =
        let viewStack =
            this.usePassed (Store.shared.ViewStack)

        {
            Stack = viewStack
            AddAction =
                fun action ->
                    match action with
                    | Message content ->
                        viewStack.Current @ [ createMessage content ]
                        |> viewStack.Set
                    | Subcomponent views ->
                        viewStack.Current @ views |> viewStack.Set
                    | Effects effects -> applyEffects effects
                    | Nothing -> ()
        }
