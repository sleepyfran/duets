module Duets.Simulation.Shop

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
        paymentEffects @ [ ItemAddedToCharacterInventory item ] |> Ok
    | Error error -> Error error

/// Attempts to buy a car from a dealer, checking if the character has enough
/// money. If the purchase is successful, the car is placed in the street in
/// front of the current position, so the function expects the character to
/// be in a dealer.
let buyCar state (item, price) =
    let characterAccount = Queries.Bank.playableCharacterAccount state
    let paymentStatus = expense state characterAccount price

    match paymentStatus with
    | Ok paymentEffects ->
        let cityId, placeId, _ = Queries.World.currentCoordinates state

        let streetId =
            Queries.World.currentPlace state |> Queries.World.firstExitOfPlace

        let streetPartition =
            Queries.World.findPlaceStreetPart cityId placeId streetId

        paymentEffects
        @ [ ItemAddedToWorld((cityId, streetId, streetPartition), item) ]
        |> Ok
    | Error error -> Error error
