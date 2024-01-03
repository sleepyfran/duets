module Duets.Simulation.Tests.Merchandise.Order

open NUnit.Framework
open FsUnit
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Merchandise.Order

let private dummyMerch =
    { Item =
        { Brand = "DuetsMerch"
          Name = "T-shirt"
          Properties = [ Wearable TShirt ] }
      MinPieces = 100<quantity>
      MaxPieces = 1000<quantity>
      PricePerPiece = 10m<dd> }

let private stateWithBandFunds =
    dummyState
    |> State.Bank.setBalance
        dummyBandBankAccount.Holder
        (Incoming(1000m<dd>, 1000m<dd>)) (* Enough to cover an order of 100 items. *)

[<Test>]
let ``ordering less than the minimum results in an error`` () =
    orderMerch dummyState dummyMerch 10<quantity>
    |> Result.unwrapError
    |> should be (ofCase <@ MinNotReached @>)

[<Test>]
let ``ordering more than the maximum results in an error`` () =
    orderMerch dummyState dummyMerch 1001<quantity>
    |> Result.unwrapError
    |> should be (ofCase <@ MaxSurpassed @>)

[<Test>]
let ``ordering more than the band can afford results in an error`` () =
    orderMerch stateWithBandFunds dummyMerch 101<quantity>
    |> Result.unwrapError
    |> should be (ofCase <@ NotEnoughFunds @>)

[<Test>]
let ``ordering items creates expenses`` () =
    orderMerch stateWithBandFunds dummyMerch 100<quantity>
    |> Result.unwrap
    |> List.filter (function
        | MoneyTransferred _ -> true
        | _ -> false)
    |> should
        equal
        [ MoneyTransferred(
              dummyBandBankAccount.Holder,
              Outgoing(1000m<dd>, 0m<dd>)
          ) ]

[<Test>]
let ``ordering adds a deliverable item to the merch workshop in a week`` () =
    let coords = Queries.World.currentCoordinates stateWithBandFunds
    let nextWeek = dummyToday |> Calendar.Ops.addDays 7

    let effect =
        orderMerch stateWithBandFunds dummyMerch 100<quantity>
        |> Result.unwrap
        |> List.filter (function
            | ItemAddedToWorld _ -> true
            | _ -> false)
        |> List.exactlyOne

    match effect with
    | ItemAddedToWorld(targetCoords, item) ->
        targetCoords |> should equal coords

        item.Properties
        |> should
            contain
            (Deliverable(
                nextWeek,
                DeliverableItem.Description(dummyMerch.Item, 100<quantity>)
            ))
    | _ -> failwith "Unexpected effect"

[<Test>]
let ``ordering schedules a notification for the delivery time`` () =
    let nextWeek = dummyToday |> Calendar.Ops.addDays 7

    let effect =
        orderMerch stateWithBandFunds dummyMerch 100<quantity>
        |> Result.unwrap
        |> List.filter (function
            | NotificationScheduled _ -> true
            | _ -> false)
        |> List.exactlyOne

    match effect with
    | NotificationScheduled(date,
                            dayMoment,
                            Notification.DeliveryArrived(_,
                                                         _,
                                                         DeliveryType.Merchandise)) ->
        (date, nextWeek |> Calendar.Transform.resetDayMoment) ||> should equal
        dayMoment |> should equal EarlyMorning
    | _ -> failwith "Unexpected effect"
