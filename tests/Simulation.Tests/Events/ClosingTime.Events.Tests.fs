module Duets.Simulation.Tests.Events.ClosingTime

open Duets.Data.World
open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Data
open Duets.Entities
open Duets.Simulation

let private cafe =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Cafe |> List.head

let private concertSpace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.ConcertSpace
    |> List.head

let private cafeCareer =
    { Id = Barista
      CurrentStage = (Careers.BaristaCareer.stages |> List.head)
      Location = Prague, cafe.Id, Ids.Common.cafe }

let nearClosingTime = dummyToday |> Calendar.Transform.changeDayMoment Evening
let closingTime = nearClosingTime |> Calendar.Query.next
let timeAdvancedEffect = TimeAdvanced(closingTime)

let private initialState =
    dummyState
    |> State.World.move Prague cafe.Id "0"
    |> State.Calendar.setTime nearClosingTime

[<Test>]
let ``tick of advance day moment kicks the character out of the place if it has closed``
    ()
    =
    Simulation.tickOne initialState timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``tick of advance day moment does not kick the character if they work there``
    ()
    =
    let state = initialState |> State.Career.set (Some cafeCareer)

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 0

[<Test>]
let ``tick of advance day moment does not kick the character if they had a concert there``
    ()
    =
    let concert =
        Concert.generator
            { From = dummyToday
              To = dummyToday
              City = Prague
              Venue = cafe.Id
              DayMoment = Evening }
        |> Gen.sample 1 1
        |> List.head

    let band = Queries.Bands.currentBand initialState

    let state =
        initialState
        |> State.Concerts.addPastConcert
            band
            (PerformedConcert(concert, 100<quality>))

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 0

[<Test>]
let ``tick of advance day moment kicks out the character if they had a concert there but it was more than a day ago``
    ()
    =
    let concert =
        Concert.generator
            { From = dummyToday |> Calendar.Ops.addDays -5<days>
              To = dummyToday |> Calendar.Ops.addDays -3<days>
              City = Prague
              Venue = cafe.Id
              DayMoment = Evening }
        |> Gen.sample 1 1
        |> List.head

    let band = Queries.Bands.currentBand initialState

    let state =
        initialState
        |> State.Concerts.addPastConcert
            band
            (PerformedConcert(concert, 100<quality>))

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``tick of advance day moment kicks out the character if they have a concert scheduled there but it is in more than a day``
    ()
    =
    let concert =
        Concert.generator
            { From = dummyToday |> Calendar.Ops.addDays 3<days>
              To = dummyToday |> Calendar.Ops.addDays 5<days>
              City = Prague
              Venue = cafe.Id
              DayMoment = Evening }
        |> Gen.sample 1 1
        |> List.head

    let band = Queries.Bands.currentBand initialState

    let state =
        initialState
        |> State.Concerts.addScheduledConcert
            band
            (ScheduledConcert(concert, dummyToday))

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``tick of advance day moment kicks out the character if they had a concert but was in a different place``
    ()
    =
    let concert =
        Concert.generator
            { From = dummyToday
              To = dummyToday
              City = Sydney
              Venue = cafe.Id
              DayMoment = Evening }
        |> Gen.sample 1 1
        |> List.head

    let band = Queries.Bands.currentBand initialState

    let state =
        initialState
        |> State.Concerts.addPastConcert
            band
            (PerformedConcert(concert, 100<quality>))

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 1

[<Test>]
let ``tick of advance day moment kicks out the character if they have a concert scheduled but is in a different place``
    ()
    =
    let concert =
        Concert.generator
            { From = dummyToday
              To = dummyToday
              City = Sydney
              Venue = cafe.Id
              DayMoment = Evening }
        |> Gen.sample 1 1
        |> List.head

    let band = Queries.Bands.currentBand initialState

    let state =
        initialState
        |> State.Concerts.addScheduledConcert
            band
            (ScheduledConcert(concert, dummyToday))

    Simulation.tickOne state timeAdvancedEffect
    |> fst
    |> List.filter (function
        | PlaceClosed _ -> true
        | _ -> false)
    |> should haveLength 1
