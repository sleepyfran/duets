module Duets.Simulation.Tests.Careers.Employment

open Duets.Data.World
open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Data
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Careers

let private barPlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Bar |> List.head

let private cafePlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.Cafe |> List.head

let private radioStudioPlace =
    Queries.World.placesByTypeInCity Prague PlaceTypeIndex.RadioStudio
    |> List.head

let private baristaJob =
    { Id = Barista
      CurrentStage = (Careers.BaristaCareer.stages |> List.head)
      Location = Prague, cafePlace.Id, Ids.Common.cafe }

let private bartenderJob =
    { Id = Bartender
      CurrentStage = (Careers.BartenderCareer.stages |> List.head)
      Location = Prague, barPlace.Id, Ids.Common.bar }

let private radioHostJob =
    { Id = RadioHost
      CurrentStage = (Careers.RadioHostCareer.stages |> List.head)
      Location = Prague, radioStudioPlace.Id, Ids.Studio.recordingRoom }

[<Test>]
let ``joining a job with no previous job accepts the career`` () =
    let state = State.generateOne State.defaultOptions
    let effects = Employment.acceptJob state baristaJob
    effects |> should haveLength 1

    effects
    |> List.head
    |> should equal (CareerAccept(state.PlayableCharacterId, baristaJob))

[<Test>]
let ``joining a job with a previous job leaves the previous before accepting``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                Career = Some bartenderJob }

    let effects = Employment.acceptJob state baristaJob
    effects |> should haveLength 2

    effects
    |> should
        equal
        [ CareerLeave(state.PlayableCharacterId, bartenderJob)
          CareerAccept(state.PlayableCharacterId, baristaJob) ]

[<Test>]
let ``joining a job that requires special entrance gives the item when accepting career``
    ()
    =
    let state = State.generateOne State.defaultOptions

    let effects = Employment.acceptJob state radioHostJob
    effects |> should haveLength 2

    effects
    |> List.head
    |> should be (ofCase <@ ItemAddedToCharacterInventory @>)

    effects
    |> List.item 1
    |> should equal (CareerAccept(state.PlayableCharacterId, radioHostJob))

[<Test>]
let ``joining a job with a previous job in a place that required special entrance removes item whena accepting new career``
    ()
    =
    let state =
        State.generateOne
            { State.defaultOptions with
                Career = Some radioHostJob }

    let effects = Employment.acceptJob state baristaJob
    effects |> should haveLength 3

    effects
    |> List.head
    |> should equal (CareerLeave(state.PlayableCharacterId, radioHostJob))

    effects
    |> List.item 1
    |> should be (ofCase <@ ItemRemovedFromCharacterInventory @>)

    effects
    |> List.item 2
    |> should equal (CareerAccept(state.PlayableCharacterId, baristaJob))
