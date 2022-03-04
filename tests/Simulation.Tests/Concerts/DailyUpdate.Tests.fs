module Simulation.Tests.Concerts.DailyUpdate

open NUnit.Framework
open FsCheck
open FsUnit
open Test.Common
open Test.Common.Generators

open Aether
open Entities
open Simulation

let actAndGetConcert state =
    Concerts.DailyUpdate.dailyUpdate state
    |> fun effects ->
        match effects with
        | ConcertUpdated (_, concert) :: _ -> Concert.fromScheduled concert
        | _ -> failwith "Not possible"

[<Test>]
let ``generates as many effects as concerts are scheduled`` () =
    State.generateN
        { State.defaultOptions with
              FutureConcertsToGenerate = 10
              VenueGen = Gen.constant dummyVenue }
        100
    |> List.iter
        (fun state ->
            let effects = Concerts.DailyUpdate.dailyUpdate state

            let band =
                Simulation.Queries.Bands.currentBand state

            let concerts =
                Optic.get (Lenses.FromState.Concerts.allByBand_ band.Id) state
                |> Option.get

            List.length effects
            |> should equal (Set.count concerts.ScheduledEvents))

[<Test>]
let ``generates sold tickets based on band's fame, venue capacity, last time visit, ticket price and days until the concert``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert dummyConcert)

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 12

[<Test>]
let ``sold tickets get lower when band fame is lower`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 5
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert
                { dummyConcert with
                      TicketPrice = 2<dd> })

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 2

[<Test>]
let ``sold tickets get added to the previously sold tickets`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert { dummyConcert with TicketsSold = 10 })

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 22

[<Test>]
let ``daily sold tickets are calculated based on how many days are left until the concert``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert
                { dummyConcert with
                      Date = dummyToday.AddDays(60) })
        |> actAndGetConcert

    concert.TicketsSold |> should equal 6

let actAndGetConcertWithPrice price =
    State.generateOne
        { State.defaultOptions with
              BandFame = 25
              VenueGen = Gen.constant dummyVenue }
    |> State.Concerts.addScheduledConcert
        dummyBand
        (ScheduledConcert
            { dummyConcert with
                  TicketPrice = price })
    |> actAndGetConcert

[<Test>]
let ``sold tickets decrease if ticket price gets close to the price cap`` () =
    let concert = actAndGetConcertWithPrice 23<dd>
    concert.TicketsSold |> should equal 10

    let concert = actAndGetConcertWithPrice 24<dd>
    concert.TicketsSold |> should equal 7

    let concert = actAndGetConcertWithPrice 25<dd>
    concert.TicketsSold |> should equal 7

[<Test>]
let ``sold tickets decrease when price goes slightly above price cap`` () =
    let concert = actAndGetConcertWithPrice 26<dd>
    concert.TicketsSold |> should equal 7

    let concert = actAndGetConcertWithPrice 27<dd>
    concert.TicketsSold |> should equal 7

    let concert = actAndGetConcertWithPrice 28<dd>
    concert.TicketsSold |> should equal 7

[<Test>]
let ``sold tickets decrease a lot when price goes over price cap`` () =
    let concert = actAndGetConcertWithPrice 29<dd>
    concert.TicketsSold |> should equal 4

    let concert = actAndGetConcertWithPrice 30<dd>
    concert.TicketsSold |> should equal 3

    let concert = actAndGetConcertWithPrice 50<dd>
    concert.TicketsSold |> should equal 0


[<Test>]
let ``sold tickets are capped to venue capacity`` () =
    let concert =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert { dummyConcert with TicketsSold = 1500 })
        |> actAndGetConcert

    concert.TicketsSold |> should equal 1500

[<Test>]
let ``sold tickets should not decrease out of the normal cap when last visit to the city was more than 180 days ago``
    ()
    =
    let cityId = dummyCity.Id

    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                          From = dummyToday.AddYears(-2)
                          To = dummyToday.AddDays(-180)
                          CityId = cityId }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  PastConcertsToGenerate = 1
                  PastConcertGen = concertInCityGenerator
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert dummyConcert)
        |> actAndGetConcert

    concert.TicketsSold |> should equal 12

[<Test>]
let ``sold tickets decrease to 70% of the normal cap when last visit to the city was less than 180 days ago``
    ()
    =
    let cityId = dummyCity.Id

    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                          From = dummyToday.AddDays(-60)
                          To = dummyToday.AddDays(-20)
                          CityId = cityId }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  PastConcertsToGenerate = 1
                  PastConcertGen = concertInCityGenerator
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert dummyConcert)
        |> actAndGetConcert

    concert.TicketsSold |> should equal 9

[<Test>]
let ``sold tickets decrease to 20% of the normal cap when last visit to the city was less than 30 days ago``
    ()
    =
    let cityId = dummyCity.Id

    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                          From = dummyToday.AddDays(10)
                          To = dummyToday.AddDays(29)
                          CityId = cityId }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                  BandFame = 25
                  PastConcertsToGenerate = 5
                  PastConcertGen = concertInCityGenerator
                  VenueGen = Gen.constant dummyVenue }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert dummyConcert)
        |> actAndGetConcert

    concert.TicketsSold |> should equal 2
