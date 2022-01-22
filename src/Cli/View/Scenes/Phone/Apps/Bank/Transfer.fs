module Cli.View.Scenes.Phone.Apps.Bank.Transfer

open Agents
open Cli.View.Actions
open Cli.View.Text
open Entities
open Simulation.Bank.Operations

/// Asks for the amount that the user wants to transfer from the two accounts
/// and confirms the transaction.
let rec transferSubScene bankApp sender receiver =
    seq {
        yield
            Prompt
                { Title =
                      BankAppTransferAmount receiver
                      |> PhoneText
                      |> I18n.translate
                  Content = NumberPrompt(handleAmount bankApp sender receiver) }
    }

and handleAmount bankApp sender receiver amount =
    let state = State.get ()

    if amount > 0 then
        transfer state sender receiver (amount * 1<dd>)
        |> fun result ->
            match result with
            | Ok effects ->
                seq {
                    yield! Seq.map Effect effects
                    yield! bankApp ()
                }
            | Error (NotEnoughFunds _) ->
                seq {
                    yield
                        I18n.translate (PhoneText BankAppTransferNotEnoughFunds)
                        |> Message

                    yield! bankApp ()
                }
    else
        seq {
            yield
                I18n.translate (PhoneText BankAppTransferNothingTransferred)
                |> Message

            yield! bankApp ()
        }
