module rec Duets.Simulation.EffectModifiers.EffectModifiers

open Duets.Entities

/// Applies a list of possible modifiers to an effect based on the current state
/// of the game. For example, if the character currently has the `NotInspired`
/// moodlet then all song composition and improvement effects will have a lower
/// chance of success. The state passed into all of the modifiers is frozen to
/// right before the effect is applied.
let modify state effect =
    match effect with
    | GameCreated _ -> effect (* We cannot apply anything if we've just created the game. *)
    | _ -> modify' state effect

let private modify' state effect =
    [ Moodlets.modify ]
    |> List.fold (fun effect modifier -> modifier state effect) effect
