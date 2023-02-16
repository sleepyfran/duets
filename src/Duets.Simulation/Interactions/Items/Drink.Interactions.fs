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
let rec drink state item =
    let character = Queries.Characters.playableCharacter state

    match item with
    | Beer (amount, alcoholContent) ->
        drinkAlcohol character amount alcoholContent
    | Coffee amount -> drinkCoffee character amount
    | Cola _ -> Character.Attribute.add character CharacterAttribute.Energy 1
    | Lemonade _ -> []

and private drinkAlcohol character amount alcoholContent =
    let amountOfAlcohol = (float amount * alcoholContent) / 100.0

    let quantityUntilMax =
        (amountOfAlcohol / float Config.LifeSimulation.maximumAlcoholAmount)
        * 100.0
        |> Math.roundToNearest

    Character.Attribute.add
        character
        CharacterAttribute.Drunkenness
        quantityUntilMax

and private drinkCoffee character amount =
    let energyGained =
        (float amount * float Config.LifeSimulation.energyPerCoffeeMl)
        |> Math.roundToNearest

    Character.Attribute.add character CharacterAttribute.Energy energyGained
