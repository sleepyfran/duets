[<RequireQualifiedAccess>]
module Simulation.Simulation

open Entities
open Simulation.Events

type private TickState =
    { AppliedEffects: Effect list
      State: State }

let rec private tick' tickState (nextEffectFns: EffectFn list) : TickState =
    match nextEffectFns with
    | effectFn :: rest -> effectFn tickState.State |> tickEffect tickState rest
    | [] -> tickState

and private tickEffect tickState nextEffectFns effects =
    match effects with
    | [] -> tick' tickState nextEffectFns
    | effect :: restOfEffects ->
        let updatedState = State.Root.applyEffect tickState.State effect
        let associatedEffectFns = Events.associatedEffects effect

        (* Tick all the associated effects first, and pass the rest of the
           effects that come after the current one that was applied plus all
           the other effect functions that are left to be applied.
           tickAssociatedEffects will then decide what to apply and what to
           discard. *)
        tickAssociatedEffects
            { AppliedEffects = tickState.AppliedEffects @ [ effect ]
              State = updatedState }
            associatedEffectFns
            (* Prepend the rest of the effects so that they'll be processed
               before the next effects on the chain. *)
            ((fun _ -> restOfEffects) :: nextEffectFns)

and private tickAssociatedEffects tickState associatedEffects nextEffectFns =
    match associatedEffects with
    | BreakChain effectFns :: _ ->
        (* Breaking the chain means discarding the tail of associated effects
           and also the rest of the effect fns that were left to be applied. *)
        tick' tickState effectFns
    | ContinueChain effectFns :: restOfAssociatedEffects ->
        (* When continuing a chain, we pre-pend all the effect functions that
           were generated in this associated effect to the actual tail of effect
           functions that are left to be applied. *)
        effectFns @ nextEffectFns
        |> tickAssociatedEffects tickState restOfAssociatedEffects
    | [] -> tick' tickState nextEffectFns

//// Ticks the simulation by applying multiple effects, gathering its associated
/// effects and applying them as well.
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let rec tickMultiple currentState effects =
    let effectFns = fun _ -> effects

    (* TODO: Re-add time events. *)
    let tickResult =
        tick'
            { AppliedEffects = []
              State = currentState }
            (effectFns :: Events.endOfChainEffects)

    tickResult.AppliedEffects, tickResult.State

/// Same as `tickMultiple` but with one effect.
let tickOne currentState effect = tickMultiple currentState [ effect ]
