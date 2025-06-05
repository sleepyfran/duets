module Duets.Simulation.Gym.Entrance

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

/// Pays the one-time entrance fee to the gym, which deducts the amount from the
/// player's account and adds a chip to access the gym to their inventory.
let pay state amount =
    let cityId, placeId, _ = Queries.World.currentCoordinates state

    let characterAccount = Queries.Bank.playableCharacterAccount state

    expense state characterAccount amount
    |> Result.map (fun effects ->
        let entranceChip = Item.Key.createChipFor cityId placeId

        effects @ [ ItemAddedToCharacterInventory entranceChip ])
