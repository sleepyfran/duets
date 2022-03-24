namespace Simulation.Queries.World

open Aether
open Aether.Operators
open Entities
open Simulation

module ConcertSpace =
    /// Returns all concert spaces in the given city.
    let allInCity state cityId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.map List.ofSeq
        |> Option.defaultValue []
        |> List.choose
            (fun kvp ->
                match kvp.Value with
                | ConcertPlace place -> Some(kvp.Key, place.Space)
                | _ -> None)
        |> List.distinctBy fst

    /// Retrieves a concert space given its node ID and the ID of the city
    /// that contains it.
    let byId state cityId nodeId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.defaultValue Map.empty
        |> Map.tryFind nodeId
        |> Option.bind
            (fun node ->
                match node with
                | ConcertPlace place -> Some place.Space
                | _ -> None)

    let private closestRoom state matchRoom =
        let position = Common.currentPosition state

        match position.NodeContent with
        | ConcertPlace place ->
            let (currentPlaceId, currentNodeId) =
                match position.Coordinates with
                | Room (placeId, roomId) -> (placeId, roomId)
                | Node placeId -> (placeId, place.Rooms.StartingNode)

            Common.availableDirections currentNodeId place.Rooms
            |> List.tryPick
                (fun (_, nodeId) ->
                    Common.contentOf place.Rooms nodeId
                    |> matchRoom currentPlaceId nodeId)
        | _ -> None

    /// Finds the closest backstage space connected to the current position.
    let closestBackstage state =
        closestRoom
            state
            (fun currentPlaceId nodeId room ->
                match room with
                | Backstage -> Room(currentPlaceId, nodeId) |> Some
                | _ -> None)

    /// Finds the closest stage connected to the current position.
    let closestStage state =
        closestRoom
            state
            (fun currentPlaceId nodeId room ->
                match room with
                | Stage -> Room(currentPlaceId, nodeId) |> Some
                | _ -> None)

    /// Returns whether the character has a concert scheduled in the current venue
    /// and it's right in this day moment and therefore can access or not the stage.
    let canEnterStage state placeId =
        let timeRightNow = Queries.Calendar.today state
        let band = Queries.Bands.currentBand state

        Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
        |> Option.map
            (fun (ScheduledConcert concert) ->
                let dateWithDayMoment =
                    concert.Date
                    |> Calendar.Transform.changeDayMoment concert.DayMoment

                dateWithDayMoment = timeRightNow)
        |> Option.defaultValue false

    /// Returns whether the character has a concert scheduled in the current
    /// venue today and therefore can access or not the backstage.
    let canEnterBackstage state placeId =
        let band = Queries.Bands.currentBand state

        Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
        |> Option.isSome
