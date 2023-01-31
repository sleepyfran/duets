[<AutoOpen>]
module Simulation.Events.Types

open Entities

/// A function that when given the current game state, returns a list of effects
/// that happen after the action.
type EffectFn = State -> Effect list

/// Defines what kind of effects are wrapped in the associated effects. BreakChain
/// should apply only the given list and discard the rest, continue chain executes
/// the given effects and then the rest.
type AssociatedEffectType =
    | BreakChain of EffectFn list
    | ContinueChain of EffectFn list
