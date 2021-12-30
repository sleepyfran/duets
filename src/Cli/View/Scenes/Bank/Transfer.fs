module Cli.View.Scenes.Bank.Transfer

open Cli.View.Actions
open Cli.View.Text
open Entities
open Simulation.Bank.Operations

/// Asks for the amount that the user wants to transfer from the two accounts
/// and confirms the transaction.
let rec transferSubScene sender receiver =
    seq {
        yield
            Prompt
                { Title =
                      BankTransferAmount receiver
                      |> BankText
                      |> I18n.translate
                  Content = NumberPrompt(handleAmount sender receiver) }
    }

and handleAmount sender receiver amount =
    let state = State.Root.get ()

    transfer state sender receiver (amount * 1<dd>)
    |> fun result ->
        match result with
        | Ok effects ->
            seq {
                yield! Seq.map Effect effects
                yield Scene Bank
            }
        | Error (NotEnoughFunds _) ->
            seq {
                yield
                    I18n.translate (BankText BankTransferNotEnoughFunds)
                    |> Message

                yield Scene Bank
            }
