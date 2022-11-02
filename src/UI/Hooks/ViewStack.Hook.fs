module UI.Hooks.ViewStack

open Agents
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Simulation
open UI
open UI.Types

type ViewStack = {
    Stack: IReadable<IView list>
    AddAction: InGameAction -> unit
}

let private createMessage (content: IView list) : IView =
    RichTextBlock.create [ RichTextBlock.inlines content ]

let private applyEffects effects =
    effects
    |> List.iter (fun effect ->
        let effects, state =
            Simulation.tick (State.get ()) effect

        State.set state

        effects |> Seq.iter Log.append
    (* TODO: Display effects! *)
    )

type IComponentContext with
    /// Provides a function that can set the current scene to the specified
    /// one and performs any side-effect related to that (for example, saving
    /// the game).
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
