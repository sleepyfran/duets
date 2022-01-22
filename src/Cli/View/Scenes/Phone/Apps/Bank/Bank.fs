module Cli.View.Scenes.Phone.Apps.Bank.Root

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation.Queries

let private rehearsalOptions =
    [ { Id = "transfer_to_band"
        Text = I18n.translate (PhoneText BankAppTransferToBand) }
      { Id = "transfer_from_band"
        Text = I18n.translate (PhoneText BankAppTransferFromBand) } ]

/// Creates the bank scene which allows to transfer money between accounts.
let rec bankApp () =
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
            BankAppWelcome(characterBalance, bandBalance)
            |> PhoneText
            |> I18n.translate
            |> Message

        yield
            Prompt
                { Title = I18n.translate (PhoneText BankAppPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = rehearsalOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                <| processSelection characterAccount bandAccount
                            BackText = I18n.translate (CommonText CommonBack) } }
    }

and private processSelection characterAccount bandAccount choice =
    seq {
        match choice.Id with
        | "transfer_to_band" ->
            yield!
                Transfer.transferSubScene bankApp characterAccount bandAccount
        | "transfer_from_band" ->
            yield!
                Transfer.transferSubScene bankApp bandAccount characterAccount
        | _ -> yield NoOp
    }
