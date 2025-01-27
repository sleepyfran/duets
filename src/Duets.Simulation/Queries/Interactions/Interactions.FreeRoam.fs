namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Simulation

module FreeRoam =
    let private getMovementInteractions roomId place =
        Queries.World.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, destinationId)
            |> Interaction.FreeRoam)

    let private getEnterInteractions state roomId place =
        match place.PlaceType with
        | PlaceType.Street ->
            (* When the place is a street the ID of the place matches the street. *)
            let street = Queries.World.streetInCurrentCity place.Id state

            let filteredPlaces =
                street.Places
                |> List.filter (fun place ->
                    match place.PlaceType with
                    | Street -> false
                    | _ -> true)

            let places =
                match street.Type with
                | StreetType.OneWay -> filteredPlaces
                | StreetType.Split(_, splits) ->
                    (*
                    This can be confusing, but the current split is the ID of
                    the room, encoded as an integer in a string.
                    *)
                    let currentSplit = int roomId

                    filteredPlaces
                    |> List.splitInto splits
                    |> List.item currentSplit

            places
            |> List.map (FreeRoamInteraction.Enter >> Interaction.FreeRoam)
        | _ -> []

    let getGoToInteractions state place =
        (*
        The player can only navigate to connecting streets from another street.
        *)
        match place.PlaceType with
        | PlaceType.Street ->
            let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

            let connectingStreets =
                zone.Streets.Connections
                |> Map.tryFind place.Id
                |> Option.defaultValue Map.empty
                |> Map.toList
                |> List.choose (fun (_, connectingStreetId) ->
                    zone.Streets.Nodes |> Map.tryFind connectingStreetId)

            [ FreeRoamInteraction.GoToStreet connectingStreets
              |> Interaction.FreeRoam ]
        | _ -> []

    let private getOutInteractions roomId place =
        place.Exits
        |> Map.tryFind roomId
        |> Option.map (FreeRoamInteraction.GoOut >> Interaction.FreeRoam)
        |> Option.toList

    let private getLookInteraction state itemsInPlace =
        let knownPeople, unknownPeople =
            Queries.World.peopleInCurrentPlace state

        FreeRoamInteraction.Look(itemsInPlace, knownPeople, unknownPeople)

    let private getNavigationInteractions state (_, _, roomId) place =
        let enterInteractions = getEnterInteractions state roomId place

        let goToInteractions = getGoToInteractions state place

        let withinPlaceMovementInteractions =
            getMovementInteractions roomId place

        let exitInteraction = getOutInteractions roomId place

        withinPlaceMovementInteractions
        @ exitInteraction
        @ enterInteractions
        @ goToInteractions

    let internal interactions
        state
        currentCoords
        currentPlace
        inventory
        itemsInPlace
        =
        getNavigationInteractions state currentCoords currentPlace
        @ [ FreeRoamInteraction.Inventory inventory |> Interaction.FreeRoam
            getLookInteraction state itemsInPlace |> Interaction.FreeRoam
            FreeRoamInteraction.Map |> Interaction.FreeRoam
            FreeRoamInteraction.Phone |> Interaction.FreeRoam
            FreeRoamInteraction.Wait |> Interaction.FreeRoam ]
