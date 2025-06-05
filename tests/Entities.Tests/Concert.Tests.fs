module Duets.Entities.Tests.Concert

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Entities.Calendar

let private createDummyConcert date =
    Concert.create
        date
        Night
        Prague
        dummyVenue.Id
        10m
        ParticipationType.Headliner

let private concertOnSummer10 =
    createDummyConcert (Shorthands.Summer 10<days> 2023<years>)

let private concertOnAutumn10 =
    createDummyConcert (Shorthands.Autumn 10<days> 2023<years>)

let private concertOnAutumn20 =
    createDummyConcert (Shorthands.Autumn 20<days> 2023<years>)

let private concertOnAutumn30 =
    createDummyConcert (Shorthands.Autumn 30<days> 2023<years>)

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
        Concert.Timeline.addScheduled (scheduled concertOnAutumn10) []

    scheduledEvents |> should haveLength 1

    scheduledEvents |> List.head |> should equal (scheduled concertOnAutumn10)

[<Test>]
let ``adding a second scheduled concert that is scheduled after the first one pushes it to the end of the list``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnAutumn10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnAutumn20)

    scheduledEvents |> should haveLength 2

    scheduledEvents |> List.head |> should equal (scheduled concertOnAutumn10)

    scheduledEvents |> List.item 1 |> should equal (scheduled concertOnAutumn20)

[<Test>]
let ``adding a third concert that is before the rest of them adds it to the front of the list``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnAutumn10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnAutumn20)
        |> Concert.Timeline.addScheduled (scheduled concertOnSummer10)

    scheduledEvents |> should haveLength 3

    scheduledEvents |> List.head |> should equal (scheduled concertOnSummer10)

    scheduledEvents |> List.item 1 |> should equal (scheduled concertOnAutumn10)

    scheduledEvents |> List.item 2 |> should equal (scheduled concertOnAutumn20)

[<Test>]
let ``adding a concert that is scheduled between the two first elements adds it between those two elements``
    ()
    =
    let scheduledEvents =
        Concert.Timeline.addScheduled (scheduled concertOnSummer10) []
        |> Concert.Timeline.addScheduled (scheduled concertOnAutumn20)
        |> Concert.Timeline.addScheduled (scheduled concertOnAutumn10)

    scheduledEvents |> should haveLength 3

    scheduledEvents |> List.head |> should equal (scheduled concertOnSummer10)

    scheduledEvents |> List.item 1 |> should equal (scheduled concertOnAutumn10)

    scheduledEvents |> List.item 2 |> should equal (scheduled concertOnAutumn20)

// =============================================================================
// Past concerts
// =============================================================================

[<Test>]
let ``adding a concert when the list is empty returns a list with that single concert``
    ()
    =
    let pastEvents = Concert.Timeline.addPast (past concertOnAutumn10) []

    pastEvents |> should haveLength 1
    pastEvents |> List.head |> should equal (past concertOnAutumn10)

[<Test>]
let ``adding a second concert that happened before the first one adds it to the end of the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnAutumn10) []
        |> Concert.Timeline.addPast (past concertOnSummer10)

    pastEvents |> should haveLength 2

    pastEvents |> List.head |> should equal (past concertOnAutumn10)

    pastEvents |> List.item 1 |> should equal (past concertOnSummer10)

[<Test>]
let ``adding a third concert that happened after the first two adds it to the beginning of the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnAutumn10) []
        |> Concert.Timeline.addPast (past concertOnSummer10)
        |> Concert.Timeline.addPast (past concertOnAutumn20)

    pastEvents |> should haveLength 3

    pastEvents |> List.head |> should equal (past concertOnAutumn20)

    pastEvents |> List.item 1 |> should equal (past concertOnAutumn10)

    pastEvents |> List.item 2 |> should equal (past concertOnSummer10)

[<Test>]
let ``adding a concert that is between the two first elements adds it as the second element in the list``
    ()
    =
    let pastEvents =
        Concert.Timeline.addPast (past concertOnSummer10) []
        |> Concert.Timeline.addPast (past concertOnAutumn20)
        |> Concert.Timeline.addPast (past concertOnAutumn10)

    pastEvents |> should haveLength 3

    pastEvents |> List.head |> should equal (past concertOnAutumn20)

    pastEvents |> List.item 1 |> should equal (past concertOnAutumn10)

    pastEvents |> List.item 2 |> should equal (past concertOnSummer10)
