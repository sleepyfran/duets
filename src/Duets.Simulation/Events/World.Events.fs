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

let private ifCoordsDiffer previousCoords currentCoords f =
    if previousCoords = currentCoords then [] else f ()

let private removeItemsRequiredByPlaceIfNeeded previousCoords currentCoords _ =
    ifCoordsDiffer previousCoords currentCoords (fun () ->
        let previousCityId, previousPlaceId, _ = previousCoords

        let previousPlace =
            Queries.World.placeInCityById previousCityId previousPlaceId

        previousPlace.Rooms.Nodes
        |> List.ofMapValues
        |> List.collect (fun room ->
            match room.RequiredItemsForEntrance with
            | Some requiredItems -> requiredItems.Items
            | _ -> [])
        |> List.map ItemRemovedFromInventory)

let private generateNpcs previousCoords currentCoords state =
    ifCoordsDiffer previousCoords currentCoords (fun () ->
        let cityId, placeId, _ = currentCoords
        let place = Queries.World.placeInCityById cityId placeId

        World.Population.generateForPlace cityId place state |> List.singleton)

/// Runs all the events associated with effects of world movement. For example,
/// required items from rooms have to be removed from the character's inventory
/// when they move away from the room.
let internal run effect =
    match effect with
    | WorldEnter(Diff(before, after)) ->
        [ removeItemsIfNeeded before after ] |> ContinueChain |> Some
    | WorldMoveTo(Diff(before, after)) ->
        [ removeItemsRequiredByPlaceIfNeeded before after
          generateNpcs before after ]
        |> ContinueChain
        |> Some
    | _ -> None
