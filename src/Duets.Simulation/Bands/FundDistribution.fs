[<RequireQualifiedAccess>]
module Duets.Simulation.Bands.FundDistribution

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

let distribute state (amount: Amount) =
    let characterAccount = Queries.Bank.playableCharacterAccount state
    let band = Queries.Bands.currentBand state
    let bandAccount = Band band.Id
    let bandAccountBalance = Queries.Bank.balanceOf state bandAccount

    let characterAmount = amount / (band.Members |> List.length |> decimal)

    let incomeEffect = income state characterAccount characterAmount

    let updatedBandBalance = bandAccountBalance - amount

    withBalanceChecking
        state
        bandAccount
        amount
        [ incomeEffect
          BalanceUpdated(
              bandAccount,
              Diff(bandAccountBalance, updatedBandBalance)
          ) ]
    |> Result.map (fun effects -> effects, characterAmount)
