module Cli.Scenes.Phone.Apps.Bank.Transfer

open Agents
open Cli
open Cli.Components
open Cli.Text
open Entities
open Simulation.Bank.Operations

/// Asks for the amount that the user wants to transfer from the two accounts
/// and confirms the transaction.
let rec transferSubScene bankApp sender receiver =
    let amount =
        showNumberPrompt (Phone.bankAppTransferAmount receiver)

    if amount > 0 then
        transfer (State.get ()) sender receiver (amount * 1<dd>)
        |> fun result ->
            match result with
            | Ok effects -> effects |> List.iter Effect.apply
            | Error (NotEnoughFunds _) ->
                Phone.bankAppTransferNotEnoughFunds |> showMessage
    else
        Phone.bankAppTransferNothingTransferred
        |> showMessage

    bankApp ()
