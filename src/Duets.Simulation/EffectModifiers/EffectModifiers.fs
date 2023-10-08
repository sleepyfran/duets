module Duets.Simulation.EffectModifiers.EffectModifiers

/// Applies a list of possible modifiers to an effect based on the current state
/// of the game. For example, if the character currently has the `NotInspired`
/// moodlet then all song composition and improvement effects will have a lower
/// chance of success. The state passed into all of the modifiers is frozen to
/// right before the effect is applied.
let modify state effect =
    [ Moodlets.modify ]
    |> List.fold (fun effect modifier -> modifier state effect) effect
