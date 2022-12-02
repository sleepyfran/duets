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
        { State.defaultOptions with FutureConcertsToGenerate = 10 }
        100
    |> List.iter (fun state ->
        let effects =
            Concerts.DailyUpdate.dailyUpdate state

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
                BandFansMin = 250000
                BandFansMax = 250000 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 10

[<Test>]
let ``sold tickets get lower when band fame is lower`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 50
                BandFansMax = 50 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    Date = dummyToday.AddDays(1)
                    TicketPrice = 2m<dd> },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 17

[<Test>]
let ``sold tickets get added to the previously sold tickets`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 250000
                BandFansMax = 250000 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 10 },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 20

[<Test>]
let ``daily sold tickets are calculated based on how many days are left until the concert``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2500
                BandFansMax = 2500 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    Date = dummyToday.AddDays(15)
                    TicketPrice = 10m<dd> },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 23

let actAndGetConcertWithPrice price =
    State.generateOne
        { State.defaultOptions with
            BandFansMin = 250000
            BandFansMax = 250000 }
    |> State.Concerts.addScheduledConcert
        dummyBand
        (ScheduledConcert({ dummyConcert with TicketPrice = price }, dummyToday))
    |> actAndGetConcert

[<Test>]
let ``sold tickets decrease if ticket price gets close to the price cap`` () =
    let concert =
        actAndGetConcertWithPrice 23m<dd>

    concert.TicketsSold |> should equal 4

    let concert =
        actAndGetConcertWithPrice 24m<dd>

    concert.TicketsSold |> should equal 3

    let concert =
        actAndGetConcertWithPrice 25m<dd>

    concert.TicketsSold |> should equal 2

[<Test>]
let ``sold tickets decrease when price goes slightly above price cap`` () =
    let concert =
        actAndGetConcertWithPrice 26m<dd>

    concert.TicketsSold |> should equal 2

    let concert =
        actAndGetConcertWithPrice 27m<dd>

    concert.TicketsSold |> should equal 1

    let concert =
        actAndGetConcertWithPrice 28m<dd>

    concert.TicketsSold |> should equal 1

[<Test>]
let ``sold tickets decrease a lot when price goes over price cap`` () =
    let concert =
        actAndGetConcertWithPrice 29m<dd>

    concert.TicketsSold |> should equal 1

    let concert =
        actAndGetConcertWithPrice 30m<dd>

    concert.TicketsSold |> should equal 1

    let concert =
        actAndGetConcertWithPrice 50m<dd>

    concert.TicketsSold |> should equal 0


[<Test>]
let ``sold tickets are capped to venue capacity`` () =
    let concert =
        State.generateOne { State.defaultOptions with BandFansMax = 25 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 1500 },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 1000

[<Test>]
let ``sold tickets should not decrease out of the normal cap when last visit to the city was more than 180 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddYears(-2)
                        To = dummyToday.AddDays(-180) }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 250000
                BandFansMax = 250000
                PastConcertsToGenerate = 1
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 10

[<Test>]
let ``sold tickets decrease to 70% of the normal cap when last visit to the city was less than 180 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddDays(-60)
                        To = dummyToday.AddDays(-20) }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 250000
                BandFansMax = 250000
                PastConcertsToGenerate = 1
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 7

[<Test>]
let ``sold tickets decrease to 20% of the normal cap when last visit to the city was less than 30 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddDays(10)
                        To = dummyToday.AddDays(29) }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 250000
                BandFansMax = 250000
                PastConcertsToGenerate = 5
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 2

[<Test>]
let ``does not compute daily tickets sold as infinity when the days until the concert are 0``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2500000
                BandFansMax = 2500000 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with Date = dummyToday },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 1000
