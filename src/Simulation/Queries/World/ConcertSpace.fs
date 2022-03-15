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

    /// Returns whether the character has a concert scheduled in the current venue
    /// and therefore can access or not the stage
    let canEnterStage state placeId =
        let today = Queries.Calendar.today state
        let band = Queries.Bands.currentBand state

        let concerts =
            Queries.Concerts.allScheduled state band.Id

        concerts
        |> Seq.tryHead
        |> Option.map
            (fun (ScheduledConcert concert) ->
                let dateWithDayMoment =
                    concert.Date
                    |> Calendar.Transform.changeDayMoment concert.DayMoment

                dateWithDayMoment = today
                && concert.VenueId = placeId)
        |> Option.defaultValue false
