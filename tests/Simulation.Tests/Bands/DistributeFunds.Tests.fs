module Duets.Simulation.Tests.Bands.DistributeFunds

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands
open Duets.Simulation.Bank.Operations

let state =
    let s = State.generateOne State.defaultOptions
    let band = Queries.Bands.currentBand s
    let bandAccount = Band band.Id

    s
    |> State.Bank.setBalance
        bandAccount
        (Incoming(1000m<dd>, 1000m<dd>)) (* Only the balance is used. *)

[<Test>]
let ``distributing more than the band has returns an error`` () =
    [ 1001m<dd>; 1002m<dd>; 1200m<dd>; 1500m<dd> ]
    |> List.iter (fun amount ->
        FundDistribution.distribute state amount
        |> should be (ofCase <@ NotEnoughFunds @>))

[<Test>]
let ``distributing less than the band has returns the correct effects`` () =
    let result = FundDistribution.distribute state 500m<dd> |> Result.unwrap
    let effects = result |> fst
    let distributedAmount = result |> snd

    effects |> should haveLength 2

    let incomeEffect = effects |> List.head

    distributedAmount |> should equal 125m<dd>

    match incomeEffect with
    | MoneyEarned(_, Incoming(amount, _)) -> amount |> should equal 125m<dd>
    | _ -> failwith "Unexpected effect type"

    let balanceUpdateEffect = effects |> List.item 1

    match balanceUpdateEffect with
    | BalanceUpdated(_, Diff(_, updatedBalance)) ->
        updatedBalance |> should equal 500m<dd>
    | _ -> failwith "Unexpected effect type"
