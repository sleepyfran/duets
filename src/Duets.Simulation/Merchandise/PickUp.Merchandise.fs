module Duets.Simulation.Merchandise.PickUp

open Duets.Entities
open Duets.Simulation

/// Adds to the inventory any deliverable item from the given list and removes
/// them from the world. All the items that are not deliverable will be ignored
/// and kept in the world.
let pickUpOrder state items =
    let coords = Queries.World.currentCoordinates state
    let currentBand = Queries.Bands.currentBand state

    items
    |> List.collect (fun deliveryItem ->
        let order =
            deliveryItem
            |> Item.Property.tryPick (function
                | Deliverable(_, item) -> Some item
                | _ -> None)

        match order with
        | Some(DeliverableItem.Description(merchItem, quantity)) ->
            [ ItemAddedToBandInventory(currentBand, merchItem, quantity)
              ItemRemovedFromWorld(coords, deliveryItem) ]
        | _ -> [])
