module Simulation.Tests.Airport.BoardPlane

open NUnit.Framework
open FsUnit
open Test.Common
open Test.Common.Generators

open Entities
open Entities.SituationTypes
open Simulation
open Simulation.Flights.BoardPlane

let origin =
    Queries.World.allCities.Head.Id (* Prague*)

let destination =
    (Queries.World.allCities.Item 1).Id (* Madrid *)

let testTicket =
    { Origin = origin
      Destination = destination
      Price = 100<dd>
      Date = dummyToday
      DayMoment = Morning }

let state =
    State.generateOne State.defaultOptions

[<Test>]
let ``passSecurityCheck should drop all drinks from inventory`` () =
    let item =
        fst Data.Items.Drink.Beer.pilsnerUrquellPint

    let state =
        state
        |> State.Inventory.add item
        |> State.Inventory.add item

    let effects = passSecurityCheck state

    effects |> should haveLength 2

    effects
    |> List.iter (fun effect ->
        effect
        |> should be (ofCase <@ ItemRemovedFromInventory @>))

[<Test>]
let ``boardPlane should returns an effect that sets the situation to flying``
    ()
    =
    let effect, _ = boardPlane testTicket

    match effect with
    | SituationChanged (Airport (Flying flight)) ->
        flight |> should equal testTicket
    | _ -> failwith "Incorrect situation"

[<Test>]
let ``boardPlane should return the correct length of the flight in minutes``
    ()
    =
    let _, flightTime = boardPlane testTicket

    (* Distance Prague <-> Madrid = 1780 *)
    flightTime |> should equal 237<minute>
