module Cli.View.Scenes.Bank.Root

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Scenes.Bank
open Cli.View.Text
open Entities
open Simulation.Queries

let rehearsalOptions =
    [ { Id = "transfer_to_band"
        Text = I18n.translate (BankText BankTransferToBand) }
      { Id = "transfer_from_band"
        Text = I18n.translate (BankText BankTransferFromBand) } ]

/// Creates the bank scene which allows to transfer money between accounts.
let rec bankScene () =
    let state = State.get ()

    let characterAccount =
        Characters.playableCharacter state
        |> fun character -> character.Id
        |> Character

    let bandAccount =
        Bands.currentBand state
        |> fun band -> band.Id
        |> Band

    let characterBalance = Bank.balanceOf state characterAccount
    let bandBalance = Bank.balanceOf state bandAccount

    seq {
        yield
            BankWelcome(characterBalance, bandBalance)
            |> BankText
            |> I18n.translate
            |> Message

        yield
            Prompt
                { Title = I18n.translate (BankText BankPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = rehearsalOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                <| processSelection characterAccount bandAccount
                            BackText = I18n.translate (CommonText CommonBack) } }
    }

and processSelection characterAccount bandAccount choice =
    seq {
        match choice.Id with
        | "transfer_to_band" ->
            yield! Transfer.transferSubScene characterAccount bandAccount
        | "transfer_from_band" ->
            yield! Transfer.transferSubScene bandAccount characterAccount
        | _ -> yield NoOp
    }
