module Duets.Simulation.Navigation.Policies.Room

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns whether the character can enter in a room by checking if it requires
/// any item and disallowing the entry if the character does not have it.
let canEnter currentRoomId state cityId placeId nextRoomId =
    let room = Queries.World.roomById cityId placeId nextRoomId
    let inventory = Queries.Inventory.get state

    match room.RequiredItemsForEntrance with
    | Some requiredItems when requiredItems.ComingFrom = currentRoomId ->
        requiredItems.Items
        |> List.forall (fun item -> inventory |> List.contains item)
        |> Result.ofBool (
            RoomEntranceError.CannotEnterWithoutRequiredItems
                requiredItems.Items
        )
    | _ -> Ok()
