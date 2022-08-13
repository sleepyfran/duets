module Simulation.Shop

open Common
open Entities
open Simulation.Bank.Operations

type ItemOrderError =
    | ItemNotFound
    | NotEnoughFunds

/// Attempts to order an item checking if the character has enough money.
let order state item =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    let paymentStatus = expense state characterAccount item.Price

    match paymentStatus with
    | Ok paymentEffects -> paymentEffects @ [ InventoryItemAdded item ] |> Ok
    | Error error -> Error error

/// Attempts to order an item with the given name from the list of available
/// items. Checks for whether the name exists and if the character has enough
/// money to buy the item.
let orderByName state name availableItems =
    let foundItem =
        availableItems
        |> List.tryFind (fun item ->
            String.diacriticInsensitiveContains name item.Brand)

    match foundItem with
    | Some item ->
        order state item
        |> Result.mapError (fun _ -> NotEnoughFunds)
    | None -> Error ItemNotFound
