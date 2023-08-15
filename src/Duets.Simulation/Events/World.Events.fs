module Duets.Simulation.Events.World

open Duets.Common
open Duets.Entities
open Duets.Simulation

let private removeItemsIfNeeded previousCoords currentCoords _ =
    let cityId, placeId, previousRoomId = previousCoords
    let _, _, currentRoomId = currentCoords
    let previousRoom = Queries.World.roomById cityId placeId previousRoomId

    match previousRoom.RequiredItemsForEntrance with
    | Some requiredItems when requiredItems.ComingFrom = currentRoomId ->
        (*
        Remove all the required items when the character moves from the room
        that required the items towards the room before the items were required.
        For example: when moving from the changing room of a gym to the lobby.
        *)
        requiredItems.Items |> List.map ItemRemovedFromInventory
    | _ -> []

let private removeItemsRequiredByPlaceIfNeeded previousCoords currentCoords _ =
    let previousCityId, previousPlaceId, previousRoomId = previousCoords
    let currentCityId, currentPlaceId, currentRoomId = currentCoords

    (*
     Technically we shouldn't we raising a move for entering in different rooms,
     but nothing to do in this case.
     *)
    if previousCityId = currentCityId && previousPlaceId = currentPlaceId then
        []
    else
        let previousPlace =
            Queries.World.placeInCityById previousCityId previousPlaceId

        previousPlace.Rooms.Nodes
        |> List.ofMapValues
        |> List.collect (fun room ->
            match room.RequiredItemsForEntrance with
            | Some requiredItems -> requiredItems.Items
            | _ -> [])
        |> List.map ItemRemovedFromInventory

/// Runs all the events associated with effects of world movement. For example,
/// required items from rooms have to be removed from the character's inventory
/// when they move away from the room.
let internal run effect =
    match effect with
    | WorldEnter(Diff(before, after)) ->
        [ removeItemsIfNeeded before after ] |> ContinueChain |> Some
    | WorldMoveTo(Diff(before, after)) ->
        [ removeItemsRequiredByPlaceIfNeeded before after ]
        |> ContinueChain
        |> Some
    | _ -> None
