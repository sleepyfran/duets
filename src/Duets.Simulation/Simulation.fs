[<RequireQualifiedAccess>]
module Duets.Simulation.Simulation

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation.Events
open Duets.Simulation.Time.AdvanceTime
open Duets.Simulation.Time.InteractionMinutes

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
        (*
        Before applying the effect and gathering its associated effects, check if
        there's any current modifier that needs to be applied to the effect. For
        example, if the character is not inspired, song related effects have
        less effect.
        *)
        let effect =
            EffectModifiers.EffectModifiers.modify tickState.State effect

        let updatedState = State.Root.applyEffect tickState.State effect

        let associatedEffectFns =
            [ yield! Events.associatedEffects effect
              yield! applyTime effect updatedState ]

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

and private applyTime effect state =
    let totalTurnTime = effectMinutes effect

    if totalTurnTime > 0<minute> then
        applyTime' state totalTurnTime
    else
        // The effect didn't consume any time, so no need to do anything.
        []

and private applyTime' state totalTurnTime =
    let currentTurnMinutes = Optic.get Lenses.State.turnMinutes_ state

    let total = currentTurnMinutes + totalTurnTime

    if total >= Config.Time.minutesPerDayMoment then
        // Enough time has passed to trigger a new day moment, advance the
        // time by the number of day moments that have passed and apply those
        // to the current chain.
        let totalDayMoments =
            total / Config.Time.minutesPerDayMoment |> (*) 1<dayMoments>

        [ [ fun state -> advanceDayMoment' state totalDayMoments ]
          |> ContinueChain ]
    else if total > 0<minute> then
        // Not enough time has passed to trigger a new day moment, so just
        // update the turn time.
        [ [ (Func.toConst [ TurnTimeUpdated total ]) ] |> ContinueChain ]
    else
        []

//// Ticks the simulation by applying multiple effects, gathering its associated
/// effects and applying them as well.
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let tickMultiple currentState effects =
    let effectFns = fun _ -> effects

    let tickResult =
        tick'
            { AppliedEffects = []
              State = currentState }
            (effectFns :: Events.endOfChainEffects)

    tickResult.AppliedEffects, tickResult.State

/// Same as `tickMultiple` but with one effect.
let tickOne currentState effect = tickMultiple currentState [ effect ]
