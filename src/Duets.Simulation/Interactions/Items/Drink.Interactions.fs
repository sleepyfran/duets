module Duets.Simulation.Interactions.Drink

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Drinks the given item. Returns different effects depending on the type of
/// drink, for example:
/// - If the drink is a soda, slightly increases mood
/// - If the drink is a beer or some other alcoholic beverage, the fun begins!
///   This calculates how the drink impacts the player depending on its quantity
///   and alcoholic content, increasing the character's drunkenness.
let rec drink state item drink =
    let character = Queries.Characters.playableCharacter state

    match drink.DrinkType with
    | Beer(alcoholContent) -> drinkAlcohol character drink.Amount alcoholContent
    | Coffee amount -> drinkCoffee character amount
    | Soda -> []
    @ Items.remove state item
    @ [ Drank(item, drink) ]

and private drinkAlcohol character amount alcoholContent =
    let amountOfAlcohol = (float amount * alcoholContent) / 100.0

    let quantityUntilMax =
        (amountOfAlcohol / float Config.LifeSimulation.maximumAlcoholAmount)
        * 100.0
        |> Math.roundToNearest

    [ yield!
          Character.Attribute.add
              character
              CharacterAttribute.Drunkenness
              quantityUntilMax
      yield!
          Character.Attribute.add
              character
              CharacterAttribute.Mood
              Config.LifeSimulation.Mood.alcoholIncrease ]

and private drinkCoffee character amount =
    let energyGained =
        (float amount * float Config.LifeSimulation.energyPerCoffeeMl)
        |> Math.roundToNearest

    [ yield!
          Character.Attribute.add
              character
              CharacterAttribute.Energy
              energyGained
      yield!
          Character.Attribute.add
              character
              CharacterAttribute.Mood
              Config.LifeSimulation.Mood.coffeeIncrease ]
