module Cli.Scenes.Phone.Apps.Bank.Root

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation.Queries

type private BankMenuOptions =
    | TransferToBand
    | TransferFromBand

let private textFromOption opt =
    match opt with
    | TransferToBand -> PhoneText BankAppTransferToBand
    | TransferFromBand -> PhoneText BankAppTransferFromBand
    |> I18n.translate

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

    BankAppWelcome(characterBalance, bandBalance)
    |> PhoneText
    |> I18n.translate
    |> showMessage

    let selection =
        showOptionalChoicePrompt
            (PhoneText BankAppPrompt |> I18n.translate)
            (CommonText CommonBack |> I18n.translate)
            textFromOption
            [ TransferToBand; TransferFromBand ]

    match selection with
    | Some TransferToBand ->
        Transfer.transferSubScene bankApp characterAccount bandAccount
    | Some TransferFromBand ->
        Transfer.transferSubScene bankApp bandAccount characterAccount
    | None -> Scene.Phone
