module Duets.Simulation.Character.Attribute

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private createEffect character attribute prev curr =
    if prev = curr then
        []
    else
        [ CharacterAttributeChanged(character, attribute, Diff(prev, curr)) ]

let private attrValue character attribute =
    Optic.get (Lenses.Character.attribute_ attribute) character
    |> Option.defaultValue 0

/// Sets the given character attribute to the given value.
let set character attribute value =
    let currentAmount = attrValue character attribute

    createEffect character.Id attribute currentAmount value

/// Applies the given mapping function to the current value of the character's
/// attribute, clamping the value between 0 and 100.
let map character attribute mapping =
    let currentAmount = attrValue character attribute

    mapping currentAmount
    |> Math.clamp 0 100
    |> createEffect character.Id attribute currentAmount

/// Sums the given amount of the attribute to the character's current one. Clamping
/// the value between 0 and 100.
let add character attribute amount = map character attribute ((+) amount)

/// Same as add but automatically applying it to the current playable character.
let addToPlayable attribute amount state =
    let character = Queries.Characters.playableCharacter state
    add character attribute amount

/// Same as set but automatically applying it to the current playable character.
let setToPlayable attribute amount state =
    let character = Queries.Characters.playableCharacter state
    set character attribute amount

/// Conditionally calls map only if the condition function returns true with
/// the current character's attribute value.
let conditionalMap character attribute condition mapping =
    if condition then map character attribute mapping else []

/// Conditionally calls add only if the condition function returns true with
/// the current character's attribute value.
let conditionalAdd character attribute condition amount =
    conditionalMap character attribute condition ((+) amount)

/// Function to pass into the conditional add function that only applies the
/// value if the current amount is more than a given amount.
let moreThan character attr amount = attrValue character attr > amount

/// Function to pass into the conditional add function that only applies the
/// value if the current amount is less than a given amount.
let lessThan character attr amount = attrValue character attr < amount

/// Function to pass into the conditional add function that only applies the
/// value if the current amount is more than zero.
let moreThanZero character attr = moreThan character attr 0
