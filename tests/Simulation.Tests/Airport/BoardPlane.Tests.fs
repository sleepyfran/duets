module Duets.Simulation.Tests.Airport.BoardPlane

open NUnit.Framework
open FsUnit
open Test.Common
open Test.Common.Generators

open Duets
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Flights.Airport

let createTicket origin destination =
    { Id = Identity.create ()
      Origin = origin
      Destination = destination
      Price = 100m<dd>
      Date = dummyToday
      DayMoment = Morning
      AlreadyUsed = false }

let testTicket = createTicket Prague Madrid

let state = State.generateOne State.defaultOptions

[<Test>]
let ``passSecurityCheck should drop all drinks from inventory`` () =
    let item = fst Data.Items.Drink.Beer.pilsnerUrquellPint

    let state = state |> State.Inventory.add item |> State.Inventory.add item

    let effects = passSecurityCheck state

    effects |> should haveLength 2

    effects
    |> List.iter (fun effect ->
        effect |> should be (ofCase <@ ItemRemovedFromInventory @>))

[<Test>]
let ``boardPlane should return an effect that sets the situation to flying``
    ()
    =
    let effect, _ = boardPlane testTicket

    match effect.Head with
    | SituationChanged(Airport(Flying flight)) ->
        flight |> should equal testTicket
    | _ -> failwith "Incorrect situation"

[<Test>]
let ``boardPlane should return an effect that marks the ticket as used`` () =
    let effect, _ = boardPlane testTicket

    match effect.Item 1 with
    | FlightUpdated flight -> flight.AlreadyUsed |> should equal true
    | _ -> failwith "Incorrect situation"

[<Test>]
let ``boardPlane should return the correct length of the flight in minutes``
    ()
    =
    [ Prague, Madrid, 237<minute>
      Prague, London, 138<minute>
      Sydney, MexicoCity, 1730<minute> ]
    |> List.iter (fun (origin, destination, expected) ->
        let ticket = createTicket origin destination

        let _, flightTime = boardPlane ticket

        flightTime |> should equal expected)
