module Simulation.Events.Character.Character

open Entities
open Simulation
open Simulation.Events

/// Runs all the events associated with a character. For example, when the health
/// of the character goes below a certain threshold we should hospitalize the
/// character.
let internal run effect =
    match effect with
    | CharacterAttributeChanged (character, attribute, Diff (_, amount)) when
        attribute = CharacterAttribute.Health && amount < 10
        ->
        BreakChain [ Hospitalization.hospitalize character ] |> Some
    | SongImproved _
    | SongStarted _ ->
        ContinueChain
            [ Character.Attribute.addToPlayable CharacterAttribute.Energy -20 ]
        |> Some
    | SongPracticed _ ->
        ContinueChain
            [ Character.Attribute.addToPlayable CharacterAttribute.Energy -10 ]
        |> Some
    | TimeAdvanced _ ->
        ContinueChain
            [ Drunkenness.soberUpAfterTime
              Drunkenness.reduceHealth
              Hunger.reduceHealth ]
        |> Some
    | _ -> None
