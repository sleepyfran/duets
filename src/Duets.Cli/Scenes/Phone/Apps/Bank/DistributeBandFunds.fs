module Duets.Cli.Scenes.Phone.Apps.Bank.DistributeBandFunds

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands
open Duets.Simulation.Bank.Operations

let distributeFunds bankApp =
    let state = State.get ()

    "This will distribute the amount you choose equally among all band members"
    |> Styles.faded
    |> showMessage

    let result =
        showDecimalPrompt "How much would you like to distribute?"
        |> (*) 1m<dd>
        |> FundDistribution.distribute state

    match result with
    | Ok(effects, totalPerMember) ->
        $"You got {totalPerMember |> Styles.money} as your share of the band funds"
        |> Styles.success
        |> showMessage

        effects |> Effect.applyMultiple
    | Error(NotEnoughFunds _) ->
        Phone.bankAppTransferNotEnoughFunds |> showMessage

    bankApp ()
