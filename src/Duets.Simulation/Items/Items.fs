module Duets.Simulation.Items

open Duets.Entities
open Duets.Simulation

/// Removes an item from either the world or the player's inventory.
let remove state item =
    let coords = Queries.World.currentCoordinates state

    let location = Queries.Items.itemLocation state coords item

    match location with
    | ItemLocation.World -> [ ItemRemovedFromWorld(coords, item) ]
    | ItemLocation.Inventory -> [ ItemRemovedFromInventory item ]
    | ItemLocation.Nowhere ->
        [] (* This technically shouldn't happen, but let's just not remove the item. *)
