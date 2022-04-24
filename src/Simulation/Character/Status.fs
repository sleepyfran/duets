module Simulation.Character.Status

open Aether
open Common
open Entities

/// Sums the given amount of health to the character's current health. Raises a
/// CharacterHealthChange keeping the value between 0 and 100.
let changeHealth character amount =
    let currentHealth =
        Optic.get Lenses.Character.health_ character

    currentHealth + amount
    |> Math.clamp 0 100
    |> Tuple.two character
    |> CharacterHealthChange

/// Sums the given amount of energy to the character's current energy. Raises a
/// CharacterEnergyChange keeping the value between 0 and 100.
let changeEnergy character amount =
    let currentEnergy =
        Optic.get Lenses.Character.energy_ character

    currentEnergy + amount
    |> Math.clamp 0 100
    |> Tuple.two character
    |> CharacterEnergyChange
