module Simulation.Character.Attribute

open Aether
open Common
open Entities

/// Sums the given amount of the attribute to the character's current one.
let add character attribute amount =
    Optic.get (Lenses.Character.attribute_ attribute) character
    |> Option.defaultValue 0
    |> (+) amount
    |> Math.clamp 0 100
    |> fun updatedAmount ->
        CharacterAttributeChanged(character, attribute, updatedAmount)
