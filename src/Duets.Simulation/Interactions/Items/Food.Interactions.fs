module rec Duets.Simulation.Interactions.Food

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Eats the given item. Returns different effects depending on the type of food:
/// - For junk food, it slightly decreases health depending on the amount consumed
///   and slightly increases happiness.
let eat state item =
    let character = Queries.Characters.playableCharacter state

    match item with
    | Burger amount
    | Croissant amount
    | Chips amount
    | Fries amount
    | Nachos amount -> eatJunkFood character amount
    | BunBo amount
    | Chicken amount
    | Falafel amount
    | Fruits amount
    | GranolaBowl amount
    | Gyozas amount
    | NemCuon amount
    | PhoBo amount
    | Ramen amount
    | Salad amount
    | Sandwich amount
    | Steak amount
    | Sushi amount
    | Wakame amount -> eatNormalFood character amount

let private calculateHungerIncrease amount =
    float amount * 0.15 |> Math.roundToNearest

let private eatJunkFood character amount =
    let hungerIncrease = calculateHungerIncrease amount
    let healthDecrease = float amount / 100.0 |> Math.roundToNearest |> ((*) -1)

    [ yield!
          Character.Attribute.add
              character
              CharacterAttribute.Hunger
              hungerIncrease
      yield!
          Character.Attribute.add
              character
              CharacterAttribute.Health
              healthDecrease ]

let private eatNormalFood character amount =
    let hungerIncrease = calculateHungerIncrease amount

    Character.Attribute.add character CharacterAttribute.Hunger hungerIncrease
