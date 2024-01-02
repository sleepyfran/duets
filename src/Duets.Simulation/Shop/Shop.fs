module Duets.Simulation.Shop

open Duets.Common
open Duets.Entities
open Duets.Simulation.Bank.Operations

type ItemOrderError =
    | ItemNotFound
    | NotEnoughFunds

/// Attempts to order an item checking if the character has enough money.
let order state (item, price) =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    let paymentStatus = expense state characterAccount price

    match paymentStatus with
    | Ok paymentEffects ->
        paymentEffects @ [ ItemAddedToCharacterInventory item ]
        |> Ok
    | Error error -> Error error
