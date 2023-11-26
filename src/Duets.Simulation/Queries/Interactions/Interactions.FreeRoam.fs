namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Simulation

module FreeRoam =
    let private getMovementInteractions (_, _, roomId) place =
        Queries.World.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, destinationId)
            |> Interaction.FreeRoam)

    let private getLookInteraction state itemsInPlace =
        let peopleInPlace = Queries.World.peopleInCurrentPlace state

        let knownPeople, unknownPeople =
            peopleInPlace
            |> List.partition (fun person ->
                Queries.Relationship.withCharacter person.Id state
                |> Option.isSome)

        FreeRoamInteraction.Look(itemsInPlace, knownPeople, unknownPeople)

    let internal interactions
        state
        currentCoords
        currentPlace
        inventory
        itemsInPlace
        =
        getMovementInteractions currentCoords currentPlace
        @ [ FreeRoamInteraction.Inventory inventory |> Interaction.FreeRoam
            getLookInteraction state itemsInPlace |> Interaction.FreeRoam
            FreeRoamInteraction.Map |> Interaction.FreeRoam
            FreeRoamInteraction.Phone |> Interaction.FreeRoam
            FreeRoamInteraction.Wait |> Interaction.FreeRoam ]
