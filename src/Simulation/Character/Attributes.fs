module Simulation.Character.Attribute

open Aether
open Common
open Entities

let private createEffect character attribute prev curr =
    CharacterAttributeChanged(character, attribute, Diff(prev, curr))

/// Sums the given amount of the attribute to the character's current one. Clamping
/// the value between 0 and 100.
let add character attribute amount =
    let previousAmount =
        Optic.get (Lenses.Character.attribute_ attribute) character
        |> Option.defaultValue 0

    previousAmount
    |> (+) amount
    |> Math.clamp 0 100
    |> createEffect character attribute previousAmount

/// Applies the given mapping function to the current value of the character's
/// attribute, clamping the value between 0 and 100.
let map character attribute fn =
    let currentAmount =
        Optic.get (Lenses.Character.attribute_ attribute) character
        |> Option.defaultValue 0

    fn currentAmount
    |> Math.clamp 0 100
    |> createEffect character attribute currentAmount

/// Function to pass into the map that reduces the amount by a given quantity if
/// it is more than zero.
let addIfMoreThanZero reduceAmount currentAmount =
    if currentAmount > 0 then
        currentAmount + reduceAmount
    else
        currentAmount
