module Duets.Simulation.Tests.Concerts.Encore

open Duets.Data.World
open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation.Concerts.Live.Encore
open Duets.Simulation

let private filterFinishConcert =
    List.filter (function
        | ConcertFinished _ -> true
        | _ -> false)

let stateWithConcert =
    State.generateOne State.defaultOptions
    |> State.Concerts.addScheduledConcert
        dummyBand
        (ScheduledConcert(dummyConcert, dummyToday))

[<Test>]
let ``getting off the stage with barely any points finishes the concert`` () =
    let concert =
        { Events = []
          Points = 10<quality>
          Checklist =
            { MerchStandSetup = false
              SoundcheckDone = false }
          Concert = dummyConcert }

    let response = getOffStage stateWithConcert concert
    response.Effects |> filterFinishConcert |> should haveLength 1

[<Test>]
let ``getting off the stage after having performed an encore finishes the concert``
    ()
    =
    let concert =
        { Events = [ PerformedEncore ]
          Points = 60<quality>
          Checklist =
            { MerchStandSetup = false
              SoundcheckDone = false }
          Concert = dummyConcert }

    let response = getOffStage stateWithConcert concert
    response.Effects |> filterFinishConcert |> should haveLength 1

[<Test>]
let ``having multiple concerts scheduled does not break getting of the stage``
    ()
    =
    let pragueVenue =
        Queries.World.placesByTypeInCity Prague PlaceTypeIndex.ConcertSpace
        |> List.head

    let newYorkVenue =
        Queries.World.placesByTypeInCity NewYork PlaceTypeIndex.ConcertSpace
        |> List.head

    let tokyoVenue =
        Queries.World.placesByTypeInCity Tokyo PlaceTypeIndex.ConcertSpace
        |> List.head

    let today = dummyToday
    let tomorrow = today |> Calendar.Ops.addDays 1<days>
    let dayAfterTomorrow = tomorrow |> Calendar.Ops.addDays 1<days>

    let concert1 =
        Concert.create today Evening Prague pragueVenue.Id 10m Headliner

    let concert2 =
        Concert.create tomorrow Evening NewYork newYorkVenue.Id 10m Headliner

    let concert3 =
        Concert.create
            dayAfterTomorrow
            Evening
            Tokyo
            tokyoVenue.Id
            10m
            Headliner

    let concert =
        { Events = []
          Points = 80<quality>
          Checklist =
            { MerchStandSetup = false
              SoundcheckDone = false }
          Concert = concert1 }

    let state =
        State.generateOne State.defaultOptions
        |> State.World.move
            concert1.CityId
            concert1.VenueId
            Ids.ConcertSpace.backstage
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(concert1, today))
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(concert2, today))
        |> State.Concerts.addScheduledConcert
            dummyBand
            (ScheduledConcert(concert3, today))

    let response = getOffStage state concert
    response.Effects |> List.head |> should be (ofCase <@ WorldEnterRoom @>)
