module Duets.UI.Hooks.GameState

open Duets.Agents
open Avalonia.FuncUI
open Duets.Entities

type IComponentContext with

    /// Provides the current game state, subscribing to its changes and
    /// re-rendering the view every time the state changes. Handles the
    /// unsubscription when the view gets disposed as well.
    member this.useGameState() : State =
        (*
        We can't use the real current state as the default value because the
        initial value of a state has to be the same in each re-render, but using
        `State.empty` would make the first render happen with stub data and
        only then it would fetch the real value. Instead, we're simply going to
        use this state as a "force re-render", so that whenever the state in the
        agent changes, this hook will re-render.
        *)
        let forceRender = this.useState (Option<State>.None)

        this.useEffect (
            handler =
                (fun _ ->
                    let _ =
                        State.subscribe (Some >> forceRender.Set)
                    ()),
            triggers = [ EffectTrigger.AfterInit ]
        )

        (* Always return the current value from the agent. *)
        State.get ()
