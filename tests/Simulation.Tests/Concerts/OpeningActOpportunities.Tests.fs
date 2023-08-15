module Duets.Simulation.Tests.Concerts.OpeningActOpportunities

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts
open Duets.Simulation.Market

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

(* --------- Application. --------- *)
[<Test>]
let ``applyToConcertOpportunity returns NotEnoughFame if headliner's fame is more than 25 of the band's fame``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 1
                BandFansMax = 1 }

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should be (ofCase <@ OpeningActOpportunities.NotEnoughFame @>)

[<Test>]
let ``applyToConcertOpportunity returns NotEnoughReleases if band does not have anything released yet``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should be (ofCase <@ OpeningActOpportunities.NotEnoughReleases @>)

[<Test>]
let ``applyToConcertOpportunity returns AnotherConcertAlreadyScheduled if band already has a concert scheduled on that date``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrapError
    |> should
        be
        (ofCase <@ OpeningActOpportunities.AnotherConcertAlreadyScheduled @>)

[<Test>]
let ``applyToConcertOpportunity returns ok with effects if all checks succeed``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 20000
                BandFansMax = 30000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrap
    |> should be (ofCase <@ ConcertScheduled @>)

[<Test>]
let ``applyToConcertOpportunity returns ok if band fame is higher than headliner``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2000000
                BandFansMax = 3000000 }
        |> State.Albums.addReleased dummyBand dummyReleasedAlbum

    OpeningActOpportunities.applyToConcertOpportunity
        state
        dummyHeadlinerBand
        dummyConcert
    |> Result.unwrap
    |> should be (ofCase <@ ConcertScheduled @>)

(* --------- Generation. --------- *)
[<Test>]
let ``generate does not create any opportunities in venues that are too big or small for the band's fame level``
    ()
    =
    let genreMarket = GenreMarket.create Genres.all

    let initialState =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 3000000
                BandFansMax = 3000000 }

    let state =
        { initialState with
            GenreMarkets = genreMarket }
        |> Bands.Generation.addInitialBandsToState

    OpeningActOpportunities.generate state Prague
    |> List.iter (fun (headliner, concert) ->
        let venue = Queries.World.placeInCityById Prague concert.VenueId

        let venueCapacity =
            match venue.PlaceType with
            | ConcertSpace space -> space.Capacity
            | _ -> failwith "Concert scheduled in non-concert space"

        let headlinerFame = headliner |> Queries.Bands.estimatedFameLevel state

        match headlinerFame with
        | fame when fame < 10 ->
            venueCapacity |> should be (lessThanOrEqualTo 500)
        | fame when fame < 30 ->
            venueCapacity |> should be (lessThanOrEqualTo 5000)

            venueCapacity |> should be (greaterThanOrEqualTo 500)
        | fame when fame < 50 ->
            venueCapacity |> should be (lessThanOrEqualTo 20000)

            venueCapacity |> should be (greaterThanOrEqualTo 500)
        | _ -> venueCapacity |> should be (greaterThanOrEqualTo 500))

[<Test>]
let ``generate does not create any opportunity for a band that has more than 35 points of fame than the current band``
    ()
    =
    let genreMarket = GenreMarket.create Genres.all

    let initialState =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 100
                BandFansMax = 20000 }

    let state =
        { initialState with
            GenreMarkets = genreMarket }
        |> Bands.Generation.addInitialBandsToState

    let bandFame =
        Queries.Bands.currentBand state
        |> Queries.Bands.estimatedFameLevel state

    OpeningActOpportunities.generate state Prague
    |> List.iter (fun (headliner, _) ->
        let headlinerFame = headliner |> Queries.Bands.estimatedFameLevel state
        headlinerFame - bandFame |> should be (lessThanOrEqualTo 35))
