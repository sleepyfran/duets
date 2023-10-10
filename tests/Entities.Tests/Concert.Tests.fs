module Duets.Entities.Tests.Concert

open FsUnit
open Fugit.Months
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities

let private createDummyConcert date =
    Concert.create
        date
        Night
        Prague
        dummyVenue.Id
        10m
        ParticipationType.Headliner

let private concertOnSeptember10 = createDummyConcert (September 10 2023)

let private concertOnOctober10 = createDummyConcert (October 10 2023)

let private concertOnOctober20 = createDummyConcert (October 20 2023)

let private concertOnOctober30 = createDummyConcert (October 30 2023)

let private scheduled concert = ScheduledConcert(concert, dummyToday)

let private past concert = PerformedConcert(concert, 100<quality>)

// =============================================================================
// Scheduled concerts
// =============================================================================

[<Test>]
let ``adding a scheduled concert to the timeline when it's empty returns a list with that single concert``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnOctober10) []

    scheduledEvents |> should haveLength 1

    scheduledEvents |> List.head |> should equal (scheduled concertOnOctober10)

[<Test>]
let ``adding a second scheduled concert that is scheduled after the first one pushes it to the end of the list``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnOctober10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnOctober20)

    scheduledEvents |> should haveLength 2

    scheduledEvents |> List.head |> should equal (scheduled concertOnOctober10)

    scheduledEvents
    |> List.item 1
    |> should equal (scheduled concertOnOctober20)

[<Test>]
let ``adding a third concert that is before the rest of them adds it to the front of the list``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnOctober10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnOctober20)
        |> Concert.Timeline.addScheduled (scheduled concertOnSeptember10)

    scheduledEvents |> should haveLength 3

    scheduledEvents
    |> List.head
    |> should equal (scheduled concertOnSeptember10)

    scheduledEvents
    |> List.item 1
    |> should equal (scheduled concertOnOctober10)

    scheduledEvents
    |> List.item 2
    |> should equal (scheduled concertOnOctober20)

[<Test>]
let ``adding a concert that is scheduled between the two first elements adds it between those two elements``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnSeptember10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnOctober20)
        |> Concert.Timeline.addScheduled (scheduled concertOnOctober10)

    scheduledEvents |> should haveLength 3

    scheduledEvents
    |> List.head
    |> should equal (scheduled concertOnSeptember10)

    scheduledEvents
    |> List.item 1
    |> should equal (scheduled concertOnOctober10)

    scheduledEvents
    |> List.item 2
    |> should equal (scheduled concertOnOctober20)

// =============================================================================
// Past concerts
// =============================================================================

[<Test>]
let ``adding a concert when the list is empty returns a list with that single concert``
    ()
    =
    let pastEvents = Concert.Timeline.addPast (past concertOnOctober10) []

    pastEvents |> should haveLength 1
    pastEvents |> List.head |> should equal (past concertOnOctober10)

[<Test>]
let ``adding a second concert that happened before the first one adds it to the end of the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnOctober10) []
        |> Concert.Timeline.addPast (past concertOnSeptember10)

    pastEvents |> should haveLength 2

    pastEvents |> List.head |> should equal (past concertOnOctober10)

    pastEvents |> List.item 1 |> should equal (past concertOnSeptember10)

[<Test>]
let ``adding a third concert that happened after the first two adds it to the beginning of the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnOctober10) []
        |> Concert.Timeline.addPast (past concertOnSeptember10)
        |> Concert.Timeline.addPast (past concertOnOctober20)

    pastEvents |> should haveLength 3

    pastEvents |> List.head |> should equal (past concertOnOctober20)

    pastEvents |> List.item 1 |> should equal (past concertOnOctober10)

    pastEvents |> List.item 2 |> should equal (past concertOnSeptember10)

[<Test>]
let ``adding a concert that is between the two first elements adds it as the second element in the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnSeptember10) []
        |> Concert.Timeline.addPast (past concertOnOctober20)
        |> Concert.Timeline.addPast (past concertOnOctober10)

    pastEvents |> should haveLength 3

    pastEvents |> List.head |> should equal (past concertOnOctober20)

    pastEvents |> List.item 1 |> should equal (past concertOnOctober10)

    pastEvents |> List.item 2 |> should equal (past concertOnSeptember10)
