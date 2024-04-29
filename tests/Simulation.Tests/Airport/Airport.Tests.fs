module Duets.Simulation.Tests.Airport.Airport

open Duets.Data.World
open NUnit.Framework
open FsUnit
open Test.Common
open Test.Common.Generators

open Duets
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

let createTicket origin destination =
    { Id = Identity.create ()
      Origin = origin
      Destination = destination
      Price = 100m<dd>
      Date = dummyToday
      DayMoment = Morning
      AlreadyUsed = false }

let testTicket = createTicket Prague Madrid

let state =
    State.generateOne State.defaultOptions
    |> State.World.move Prague dummyAirport.Id Ids.Airport.securityControl

[<Test>]
let ``passSecurityCheck should move character to boarding gate`` () =
    let effects, _ = AirportPassSecurity |> runSucceedingAction state

    effects
    |> List.head
    |> should
        equal
        (WorldEnterRoom(
            Diff(
                (Prague, dummyAirport.Id, Ids.Airport.securityControl),
                (Prague, dummyAirport.Id, Ids.Airport.boardingGate)
            )
        ))

[<Test>]
let ``passSecurityCheck should drop all drinks from inventory`` () =
    let item = fst Data.Items.Drink.Beer.pilsnerUrquellPint

    let state =
        state
        |> State.Inventory.addToCharacter item
        |> State.Inventory.addToCharacter item

    let effects, _ = AirportPassSecurity |> runSucceedingAction state

    effects |> should haveLength 3

    effects
    |> List.filter (function
        | ItemRemovedBySecurity _ -> true
        | _ -> false)
    |> should haveLength 2

[<Test>]
let ``boarding a plane should set the situation to flying`` () =
    let effects, _ = AirportBoardPlane testTicket |> runSucceedingAction state

    effects |> should contain (SituationChanged(Airport(Flying testTicket)))

[<Test>]
let ``boardPlane should mark the ticket as used`` () =
    let effect, _ = AirportBoardPlane testTicket |> runSucceedingAction state

    match effect.Item 2 with
    | FlightUpdated flight -> flight.AlreadyUsed |> should equal true
    | _ -> failwith "Incorrect effect"

[<Test>]
let ``boardPlane should raise a PlaneBoarded effect with the expected time to destination``
    ()
    =
    [ Prague, Madrid, 237<minute>
      Prague, London, 138<minute>
      Sydney, MexicoCity, 1730<minute> ]
    |> List.iter (fun (origin, destination, expected) ->
        let ticket = createTicket origin destination

        let effects, _ = AirportBoardPlane ticket |> runSucceedingAction state

        effects |> should contain (PlaneBoarded(ticket, expected)))
