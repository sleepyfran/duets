module Simulation.Events.Character.Character

open Entities
open Simulation

/// Runs all the events associated with a character. For example, when the health
/// of the character goes below a certain threshold we should hospitalize the
/// character.
let internal run effect =
    match effect with
    | CharacterAttributeChanged (character, attribute, Diff (_, amount)) when
        attribute = CharacterAttribute.Health && amount < 10
        ->
        [ Hospitalization.hospitalize character ]
    | SongImproved _
    | SongStarted _ ->
        [ Character.Attribute.addToPlayable CharacterAttribute.Energy -20 ]
    | SongPracticed _ ->
        [ Character.Attribute.addToPlayable CharacterAttribute.Energy -10 ]
    | TimeAdvanced _ ->
        [ Drunkenness.soberUpAfterTime; Drunkenness.reduceHealth ]
    | _ -> []
