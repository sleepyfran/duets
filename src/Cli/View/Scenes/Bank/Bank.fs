module Cli.View.Scenes.Bank.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Queries

let rehearsalOptions =
    [ { Id = "transfer_to_band"
        Text = TextConstant BankTransferToBand }
      { Id = "transfer_from_band"
        Text = TextConstant BankTransferFromBand } ]

/// Creates the bank scene which allows to transfer money between accounts.
let rec bankScene state =
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
        yield Figlet <| TextConstant BankTitle

        yield
            BankWelcome(characterBalance, bandBalance)
            |> TextConstant
            |> Message

        yield
            Prompt
                { Title = TextConstant BankPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = rehearsalOptions
                            Handler =
                                phoneOptionalChoiceHandler
                                <| processSelection characterAccount bandAccount
                            BackText = TextConstant CommonBackToPhone } }
    }

and processSelection characterAccount bandAccount choice =
    seq {
        match choice.Id with
        | "transfer_to_band" ->
            yield
                SubScene
                <| BankTransfer(characterAccount, bandAccount)
        | "transfer_from_band" ->
            yield
                SubScene
                <| BankTransfer(bandAccount, characterAccount)
        | _ -> yield NoOp
    }
