module Duets.Cli.Scenes.Phone.Apps.Bank.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

type private BankMenuOptions =
    | TransferToBand
    | DistributeBandFunds
    | PayRental

let private textFromOption opt =
    match opt with
    | TransferToBand -> "Transfer money to band"
    | DistributeBandFunds -> "Distribute your band's funds"
    | PayRental -> Styles.warning "Pay for next month's rent"

/// Creates the bank scene which allows to transfer money between accounts.
let rec bankApp () =
    let state = State.get ()

    let characterAccount =
        Queries.Characters.playableCharacter state |> _.Id |> Character

    let bandAccount = Queries.Bands.currentBand state |> (_.Id) |> Band

    let characterBalance = Queries.Bank.balanceOf state characterAccount

    let bandBalance = Queries.Bank.balanceOf state bandAccount

    let upcomingPayments = Queries.Rentals.allUpcoming state

    Phone.bankAppWelcome characterBalance bandBalance |> showMessage

    let selection =
        showOptionalChoicePrompt
            Phone.bankAppPrompt
            Generic.back
            textFromOption
            [ TransferToBand
              DistributeBandFunds
              if List.isNotEmpty upcomingPayments then
                  PayRental ]

    match selection with
    | Some TransferToBand ->
        Transfer.transfer bankApp characterAccount bandAccount
    | Some DistributeBandFunds -> DistributeBandFunds.distributeFunds bankApp
    | Some PayRental ->
        UpcomingPayments.upcomingPayments bankApp upcomingPayments
    | None -> Scene.Phone
