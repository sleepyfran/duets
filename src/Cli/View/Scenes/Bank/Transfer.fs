module Cli.View.Scenes.Bank.Transfer

open Cli.View.Actions
open Cli.View.TextConstants
open Entities
open Simulation.Bank.Operations

/// Asks for the amount that the user wants to transfer from the two accounts
/// and confirms the transaction.
let rec transferSubScene state sender receiver =
    seq {
        yield
            Prompt
                { Title = TextConstant <| BankTransferAmount receiver
                  Content = NumberPrompt(handleAmount state sender receiver) }
    }

and handleAmount state sender receiver amount =
    transfer state sender receiver (amount * 1<dd>)
    |> fun result ->
        match result with
        | Ok effects ->
            seq {
                yield! Seq.map Effect effects
                yield SceneAfterKey Bank
            }
        | Error (NotEnoughFunds _) ->
            seq {
                yield Message <| TextConstant BankTransferNotEnoughFunds
                yield SceneAfterKey Bank
            }
