[<RequireQualifiedAccess>]
module Simulation.Simulation

open Entities
open Simulation.Time.AdvanceTime

let private getAssociatedEffects effect =
    match effect with
    | TimeAdvanced date -> [ Events.Time.run date ]
    | _ -> []
    @ Events.Skill.run effect
      @ Events.Character.Character.run effect

/// Returns how many times the time has to be advanced for the given effect.
let private timeAdvanceOfEffect effect =
    match effect with
    | AlbumRecorded _ -> 56<dayMoments> // One week
    | ConcertFinished _ -> 1<dayMoments>
    | SongImproved _ -> 1<dayMoments>
    | SongPracticed _ -> 1<dayMoments>
    | SongStarted _ -> 1<dayMoments>
    | Wait dayMoments -> dayMoments
    | _ -> 0<dayMoments>

let rec private tick' (appliedEffects, lastState) nextEffectFns =
    match nextEffectFns with
    | effectFn :: rest ->
        effectFn lastState
        |> List.fold
            (fun (currentEffectChain, currentState) effect ->
                let state =
                    State.Root.applyEffect currentState effect

                let associatedEffects =
                    getAssociatedEffects effect

                tick' (currentEffectChain @ [ effect ], state) associatedEffects)
            (appliedEffects, lastState)
        |> fun acc -> tick' acc rest
    | [] -> (appliedEffects, lastState)

/// Ticks the simulation, which applies the given effect to the state and
/// retrieves associated effects (for example: compose song -> improve skills)
/// and applies them to the state, gathering any associated effects of that one
/// until no effects are left and ticking the clock for how much the time
/// should be advanced for the given effect.
///
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let tick currentState effect =
    let timeEffects =
        [ fun state ->
              timeAdvanceOfEffect effect
              |> advanceDayMoment state.Today ]

    let effectFn = fun _ -> [ effect ]

    tick' ([], currentState) ([ effectFn ] @ timeEffects)
