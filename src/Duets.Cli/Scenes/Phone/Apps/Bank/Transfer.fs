module Duets.Cli.Scenes.Phone.Apps.Bank.Transfer

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation.Bank.Operations

/// Asks for the amount that the user wants to transfer from the two accounts
/// and confirms the transaction.
let transfer bankApp sender receiver =
    let amount = showDecimalPrompt (Phone.bankAppTransferAmount receiver)

    if amount > 0m then
        transfer (State.get ()) sender receiver (amount * 1m<dd>)
        |> fun result ->
            match result with
            | Ok effects -> effects |> List.iter Effect.apply
            | Error(NotEnoughFunds _) ->
                Phone.bankAppTransferNotEnoughFunds |> showMessage
    else
        Phone.bankAppTransferNothingTransferred |> showMessage

    bankApp ()
