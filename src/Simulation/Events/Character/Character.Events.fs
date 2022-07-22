module Simulation.Events.Character.Character

open Entities

/// Runs all the events associated with a character. For example, when the health
/// of the character goes below a certain threshold we should hospitalize the
/// character.
let internal run effect =
    match effect with
    | CharacterHealthDepleted character ->
        [ Hospitalization.hospitalize character ]
    | _ -> []
