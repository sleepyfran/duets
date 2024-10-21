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
        |> should equal (List.length concerts.ScheduledEvents))

[<Test>]
let ``generates sold tickets based on band's fame, venue capacity, last time visit, ticket price and days until the concert``
    ()
    =
    State.generateN
        { State.defaultOptions with
            BandFansMin = 2000<fans>
            BandFansMax = 9000<fans> }
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
                BandFansMin = 50<fans>
                BandFansMax = 50<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    Date = dummyToday.AddDays(1)
                    TicketPrice = 2m<dd> },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 12

[<Test>]
let ``sold tickets get added to the previously sold tickets`` () =
    let state =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 25000<fans>
                BandFansMax = 25000<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 10 },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 135

[<Test>]
let ``daily sold tickets are calculated based on how many days are left until the concert``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2500<fans>
                BandFansMax = 2500<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    Date = dummyToday.AddDays(15)
                    TicketPrice = 10m<dd> },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 24

let newYorkVenue =
    Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.ConcertSpace
    |> List.head

[<Test>]
let ``daily sold tickets are calculated based on the fans in the concert's city``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                // These fans will be added only to the city of Prague by default.
                BandFansMin = 25000<fans>
                BandFansMax = 25000<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    TicketsSold = 0
                    CityId = NewYork
                    VenueId = newYorkVenue.Id },
                dummyToday
            ))

    let concert = actAndGetConcert state
    concert.TicketsSold |> should equal 2

let actAndGetConcertWithPrice price =
    State.generateOne
        { State.defaultOptions with
            BandFansMin = 25000<fans>
            BandFansMax = 25000<fans> }
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
    let concert = actAndGetConcertWithPrice 23m<dd>

    concert.TicketsSold |> should equal 103

    let concert = actAndGetConcertWithPrice 24m<dd>

    concert.TicketsSold |> should equal 71

    let concert = actAndGetConcertWithPrice 25m<dd>

    concert.TicketsSold |> should equal 71

[<Test>]
let ``sold tickets decrease when price goes slightly above price cap`` () =
    let concert = actAndGetConcertWithPrice 26m<dd>

    concert.TicketsSold |> should equal 71

    let concert = actAndGetConcertWithPrice 27m<dd>

    concert.TicketsSold |> should equal 71

    let concert = actAndGetConcertWithPrice 28m<dd>

    concert.TicketsSold |> should equal 71

[<Test>]
let ``sold tickets decrease a lot when price goes over price cap`` () =
    let concert = actAndGetConcertWithPrice 30m<dd>

    concert.TicketsSold |> should equal 35

    let concert = actAndGetConcertWithPrice 35m<dd>

    concert.TicketsSold |> should equal 12

    let concert = actAndGetConcertWithPrice 60m<dd>

    concert.TicketsSold |> should equal 0


[<Test>]
let ``sold tickets are capped to venue capacity`` () =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMax = 25<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with TicketsSold = 1500 },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 800

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
                BandFansMin = 25000<fans>
                BandFansMax = 25000<fans>
                PastConcertsToGenerate = 1
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 125

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
                BandFansMin = 250000<fans>
                BandFansMax = 250000<fans>
                PastConcertsToGenerate = 1
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 800

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
                BandFansMin = 250000<fans>
                BandFansMax = 250000<fans>
                PastConcertsToGenerate = 5
                PastConcertGen = concertInCityGenerator }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(dummyConcert, dummyToday))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 248

[<Test>]
let ``does not compute daily tickets sold as infinity when the days until the concert are 0``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 2500000<fans>
                BandFansMax = 2500000<fans> }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with Date = dummyToday },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 800

[<Test>]
let ``computes daily tickets based on headliner if participation type is opening act``
    ()
    =
    let concert =
        State.generateOne
            { State.defaultOptions with
                BandFansMin = 250<fans>
                BandFansMax = 250<fans> }
        |> State.Bands.addSimulated
            { dummyHeadlinerBand with
                Fans = [ dummyConcert.CityId, 1200<fans> ] |> Map.ofList }
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(
                { dummyConcert with
                    Date = dummyToday
                    TicketPrice = 6m<dd>
                    ParticipationType =
                        OpeningAct(dummyHeadlinerBand.Id, 50<percent>) },
                dummyToday
            ))
        |> actAndGetConcert

    concert.TicketsSold |> should equal 208
