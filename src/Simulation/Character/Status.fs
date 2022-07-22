module Simulation.Character.Status

open Aether
open Common
open Entities

let private addStatus lens character amount =
    let current = Optic.get lens character

    current + amount |> Math.clamp 0 100

/// Sums the given amount of health to the character's current health. Raises a
/// CharacterHealthChange keeping the value between 0 and 100.
let addHealth character amount =
    let updatedHealth =
        addStatus Lenses.Character.health_ character amount

    if updatedHealth <= 5 then
        CharacterHealthDepleted character
    else
        CharacterHealthChange(character, updatedHealth)

/// Sums the given amount of energy to the character's current energy. Raises a
/// CharacterEnergyChange keeping the value between 0 and 100.
let addEnergy character amount =
    addStatus Lenses.Character.energy_ character amount
    |> Tuple.two character
    |> CharacterEnergyChange

/// Sums the given amount of mood to the character's current mood. Raises a
/// CharacterMoodChange keeping the value between 0 and 100.
let addMood character amount =
    addStatus Lenses.Character.mood_ character amount
    |> Tuple.two character
    |> CharacterMoodChange
