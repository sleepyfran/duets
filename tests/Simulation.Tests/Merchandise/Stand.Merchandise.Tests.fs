module Duets.Simulation.Tests.Merchandise.Stand

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let private queue =
    { Venue = Prague, dummyVenue.Id
      People = 42
      Until = dummyToday |> Calendar.Query.next }

let private stateWithQueue =
    { dummyState with
        MerchStandQueue = Some queue }

[<Test>]
let ``standQueue returns nothing when the band has not set up a stand`` () =
    Queries.Merch.standQueue dummyState Prague dummyVenue.Id
    |> should equal None

[<Test>]
let ``standQueue returns an empty queue while preparing with the stand set up``
    ()
    =
    let state =
        { dummyState with
            Situation =
                Concert(
                    Preparing
                        { MerchStandSetup = true
                          SoundcheckDone = false }
                ) }

    Queries.Merch.standQueue state Prague dummyVenue.Id |> should equal (Some 0)

[<Test>]
let ``standQueue returns the people waiting after the concert`` () =
    let state = { stateWithQueue with Today = queue.Until }

    Queries.Merch.standQueue state Prague dummyVenue.Id
    |> should equal (Some 42)

[<Test>]
let ``standQueue returns nothing in another venue`` () =
    Queries.Merch.standQueue stateWithQueue Prague "another-venue"
    |> should equal None

[<Test>]
let ``standQueue returns nothing once the queue is gone`` () =
    let state =
        { stateWithQueue with
            Today = Calendar.Query.next queue.Until }

    Queries.Merch.standQueue state Prague dummyVenue.Id |> should equal None
