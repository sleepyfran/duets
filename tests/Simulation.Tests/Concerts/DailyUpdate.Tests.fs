module Duets.Simulation.Tests.Concerts.DailyUpdate

open NUnit.Framework
open FsCheck
open FsUnit
open Test.Common
open Test.Common.Generators

open Aether
open Duets.Entities
open Duets
open Duets.Simulation

let actAndGetConcert state =
    Concerts.DailyUpdate.dailyUpdate state
    |> fun effects ->
        match effects with
        | ConcertUpdated(_, concert) :: _ -> Concert.fromScheduled concert
        | _ -> failwith "Not possible"

[<Test>]
let ``generates as many effects as concerts are scheduled`` () =
    State.generateN
        { State.defaultOptions with
            FutureConcertsToGenerate = 10 }
        100
    |> List.iter (fun state ->
        let effects = Concerts.DailyUpdate.dailyUpdate state

        let band = Simulation.Queries.Bands.currentBand state

        let concerts =
            Optic.get (Lenses.FromState.Concerts.allByBand_ band.Id) state
            |> Option.get

        List.length effects
        |> should equal (Set.count concerts.ScheduledEvents))

[<Test>]
let ``generates sold tickets based on band's fame, venue capacity, last time visit, ticket price and days until the concert``
    ()
    =
    State.generateN
        { State.defaultOptions with
            BandFansMin = 2000
            BandFansMax = 9000 }
        50
    |> List.iter (fun state ->
        let state =
            state
            |> State.Concerts.addScheduledConcert
                dummyBand
                (ScheduledConcert(dummyConcert, dummyToday))

        let concert = actAndGetConcert state
        concert.TicketsSold |> should be (lessThanOrEqualTo 50))

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
    concert.TicketsSold |> should equal 18

[<Test>]
let ``sold tickets get added to the previously sold tickets`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 25000
                BandFansMax = 25000 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 10 },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 137

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

    concert.TicketsSold |> should equal 27

let actAndGetConcertWithPrice price =
    State.generateOne
        { State.defaultOptions with
            BandFansMin = 25000
            BandFansMax = 25000 }
    |> State.Concerts.addScheduledConcert
        dummyBand
        (ScheduledConcert(
            { dummyConcert with
                TicketPrice = price },
            dummyToday
        ))
    |> actAndGetConcert

[<Test>]
let ``sold tickets decrease if ticket price gets close to the price cap`` () =
    let concert = actAndGetConcertWithPrice 73m<dd>

    concert.TicketsSold |> should equal 53

    let concert = actAndGetConcertWithPrice 74m<dd>

    concert.TicketsSold |> should equal 30

    let concert = actAndGetConcertWithPrice 75m<dd>

    concert.TicketsSold |> should equal 30

[<Test>]
let ``sold tickets decrease when price goes slightly above price cap`` () =
    let concert = actAndGetConcertWithPrice 76m<dd>

    concert.TicketsSold |> should equal 30

    let concert = actAndGetConcertWithPrice 77m<dd>

    concert.TicketsSold |> should equal 30

    let concert = actAndGetConcertWithPrice 78m<dd>

    concert.TicketsSold |> should equal 30

[<Test>]
let ``sold tickets decrease a lot when price goes over price cap`` () =
    let concert = actAndGetConcertWithPrice 90m<dd>

    concert.TicketsSold |> should equal 35

    let concert = actAndGetConcertWithPrice 95m<dd>

    concert.TicketsSold |> should equal 24

    let concert = actAndGetConcertWithPrice 150m<dd>

    concert.TicketsSold |> should equal 1


[<Test>]
let ``sold tickets are capped to venue capacity`` () =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMax = 25 }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 1500 },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 1200

[<Test>]
let ``sold tickets should not decrease out of the normal cap when last visit to the city was more than 180 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            (* The concert is scheduled in 30 days, thus the weird from and to. *)
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddYears(-2)
                        To = dummyToday.AddDays(-180) }
        }

    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 25000
                BandFansMax = 25000
                PastConcertsToGenerate = 1
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 127

[<Test>]
let ``sold tickets decrease to 70% of the normal cap when last visit to the city was less than 30 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            (* The concert is scheduled in 30 days, thus the weird from and to. *)
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddDays(1)
                        To = dummyToday.AddDays(15) }
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

    concert.TicketsSold |> should equal 879

[<Test>]
let ``sold tickets decrease to 20% of the normal cap when last visit to the city was less than 10 days ago``
    ()
    =
    let concertInCityGenerator =
        gen {
            (* The concert is scheduled in 30 days, thus the weird from and to. *)
            return!
                Concert.pastConcertGenerator
                    { Concert.defaultOptions with
                        From = dummyToday.AddDays(20)
                        To = dummyToday.AddDays(30) }
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

    concert.TicketsSold |> should equal 251

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

    concert.TicketsSold |> should equal 1200
