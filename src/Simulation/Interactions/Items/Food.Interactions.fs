module Simulation.Interactions.Food

open Common
open Entities
open Simulation

/// Eats the given item. Returns different effects depending on the type of food:
/// - For junk food, it slightly decreases health depending on the amount consumed
///   and slightly increases happiness.
let rec eat state item =
    let character = Queries.Characters.playableCharacter state

    match item with
    | Burger amount
    | Chips amount
    | Fries amount
    | Nachos amount -> eatJunkFood character amount
    | BunBo _
    | Gyozas _
    | NemCuon _
    | PhoBo _
    | Ramen _
    | Wakame _ -> eatNormalFood character

and private eatJunkFood character amount =
    let healthDecrease = float amount / 100.0 |> Math.roundToNearest |> ((*) -1)

    [ Character.Attribute.add character CharacterAttribute.Health healthDecrease
      Character.Attribute.add character CharacterAttribute.Mood 1 ]

and private eatNormalFood character =
    [ Character.Attribute.add character CharacterAttribute.Mood 1 ]
