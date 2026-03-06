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
    | RequestLoan
    | PayOffLoan

let private textFromOption opt =
    match opt with
    | TransferToBand -> "Transfer money to band"
    | DistributeBandFunds -> "Distribute your band's funds"
    | PayRental -> Styles.warning "Pay for next season's rent"
    | RequestLoan -> "Request a loan"
    | PayOffLoan -> Styles.warning "Pay off loan in full"

/// Creates the bank scene which allows to transfer money between accounts.
let rec bankApp () =
    let state = State.get ()

    let characterAccount =
        Queries.Characters.playableCharacter state |> _.Id |> Character

    let bandAccount = Queries.Bands.currentBand state |> (_.Id) |> Band

    let characterBalance = Queries.Bank.balanceOf state characterAccount

    let bandBalance = Queries.Bank.balanceOf state bandAccount

    let upcomingPayments = Queries.Rentals.allUpcoming state

    let activeLoan = Queries.Bank.activeLoan state
    let reputation = Queries.Bank.reputation state

    Phone.bankAppWelcome characterBalance bandBalance |> showMessage

    match activeLoan with
    | Some loan ->
        let payment = Queries.Bank.seasonalPayment loan
        Phone.bankAppLoanInfo loan.Principal loan.InterestRate payment |> showMessage
    | None -> Phone.bankAppNoLoan |> showMessage

    match reputation with
    | Flagged -> Phone.bankAppLoanFlagged |> showMessage
    | Blacklisted -> Phone.bankAppLoanBlacklisted |> showMessage
    | GoodStanding -> ()

    let selection =
        showOptionalChoicePrompt
            Phone.bankAppPrompt
            Generic.back
            textFromOption
            [ TransferToBand
              DistributeBandFunds
              if List.isNotEmpty upcomingPayments then
                  PayRental
              if Queries.Bank.canTakeLoan state then
                  RequestLoan
              if activeLoan.IsSome then
                  PayOffLoan ]

    match selection with
    | Some TransferToBand ->
        Transfer.transfer bankApp characterAccount bandAccount
    | Some DistributeBandFunds -> DistributeBandFunds.distributeFunds bankApp
    | Some PayRental ->
        UpcomingPayments.upcomingPayments bankApp upcomingPayments
    | Some RequestLoan -> LoanRequest.requestLoan bankApp
    | Some PayOffLoan -> LoanPayoff.payOffLoan bankApp
    | None -> Scene.Phone
