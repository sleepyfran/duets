namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Simulation

module FreeRoam =
    let private getMovementInteractions roomId place =
        Queries.World.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, destinationId)
            |> Interaction.FreeRoam)

    let private getOutInteractions roomId place =
        place.Exits
        |> Map.tryFind roomId
        |> Option.map (FreeRoamInteraction.GoOut >> Interaction.FreeRoam)
        |> Option.toList

    let private getLookInteraction state itemsInPlace =
        let knownPeople, unknownPeople =
            Queries.World.peopleInCurrentPlace state

        FreeRoamInteraction.Look(itemsInPlace, knownPeople, unknownPeople)

    let private getNavigationInteractions (_, _, roomId) place =
        let withinPlaceMovementInteractions =
            getMovementInteractions roomId place

        let exitInteraction = getOutInteractions roomId place

        withinPlaceMovementInteractions @ exitInteraction

    let internal interactions
        state
        currentCoords
        currentPlace
        inventory
        itemsInPlace
        =
        getNavigationInteractions currentCoords currentPlace
        @ [ FreeRoamInteraction.Inventory inventory |> Interaction.FreeRoam
            getLookInteraction state itemsInPlace |> Interaction.FreeRoam
            FreeRoamInteraction.Map |> Interaction.FreeRoam
            FreeRoamInteraction.Phone |> Interaction.FreeRoam
            FreeRoamInteraction.Wait |> Interaction.FreeRoam ]
