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
        requiredItems.Items
        |> List.map (fun item -> ItemRemovedFromCharacterInventory item)
    | _ -> []

let private ifCoordsDiffer previousCoords currentCoords f =
    if previousCoords = currentCoords then [] else f ()

let private removeItemsRequiredByPlaceIfNeeded previousCoords currentCoords _ =
    let removeItems () =
        let previousCityId, previousPlaceId, _ = previousCoords

        let previousPlace =
            Queries.World.placeInCityById previousCityId previousPlaceId

        previousPlace.Rooms.Nodes
        |> List.ofMapValues
        |> List.collect (fun room ->
            match room.RequiredItemsForEntrance with
            | Some requiredItems -> requiredItems.Items
            | _ -> [])
        |> List.map ItemRemovedFromCharacterInventory

    ifCoordsDiffer previousCoords currentCoords removeItems

let private generateNpcs previousCoords currentCoords state =
    let generateNpcs () =
        let cityId, placeId, roomId = currentCoords
        let place = Queries.World.placeInCityById cityId placeId

        World.Population.generateForPlace cityId place state |> List.singleton

    ifCoordsDiffer previousCoords currentCoords generateNpcs

let private changeSituationIfNeeded previousCoords currentCoords state =
    let changeSituation () =
        [ Concerts.Preparation.Start.startIfNeeded currentCoords ]
        |> List.collect (fun f -> f state)

    ifCoordsDiffer previousCoords currentCoords changeSituation

/// Runs all the events associated with effects of world movement. For example,
/// required items from rooms have to be removed from the character's inventory
/// when they move away from the room.
let internal run effect =
    match effect with
    | WorldEnterRoom(Diff(before, after)) ->
        [ removeItemsIfNeeded before after ] |> ContinueChain |> Some
    | WorldMoveToPlace(Diff(before, after)) ->
        [ removeItemsRequiredByPlaceIfNeeded before after
          generateNpcs before after
          changeSituationIfNeeded before after ]
        |> ContinueChain
        |> Some
    | _ -> None
