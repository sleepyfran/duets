module Duets.Simulation.Concerts.Preparation.Start

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

/// Checks whether the given coords are from a concert space in which the band
/// currently has a concert scheduled, and if so, sets the situation to
/// preparing for the concert.
let startIfNeeded coords state =
    let cityId, placeId, _ = coords
    let place = Queries.World.placeInCityById cityId placeId

    match place.PlaceType with
    | ConcertSpace _ ->
        let band = Queries.Bands.currentBand state

        let scheduledConcert =
            Queries.Concerts.scheduleForTodayInPlace state band.Id place.Id

        let currentSituation = Queries.Situations.current state

        match scheduledConcert, currentSituation with
        | Some concert, Situation.Concert _ ->
            [] (* Already inside a concert situation, do nothing. *)
        | Some concert, _ ->
            (* Transition to preparing the concert. *)
            [ Situations.preparingConcert ]
        | _ -> [] (* No concert in the current place, do nothing. *)
    | _ -> [] (* Not a concert space, do nothing. *)
