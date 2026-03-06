module Data.Savegame.Migrations.AddLoanState

open Data.Savegame.Migrations.Common
open Duets.Data.Savegame.Types
open FSharp.Data

let private defaultLoanState =
    JsonValue.Record
        [| ("ActiveLoan", JsonValue.Null)
           ("Reputation",
            JsonValue.Record [| ("Case", JsonValue.String "GoodStanding") |]) |]

/// Migration that adds the loan state to the bank part of the savegame.
let migrate (root: JsonValue) =
    let data = root.TryGetProperty("Data")

    match data with
    | Some data ->
        let oldBankData = data.TryGetProperty("BankAccounts")

        match oldBankData with
        | Some oldBankData ->
            let bankField =
                [| ("Accounts", oldBankData); ("LoanState", defaultLoanState) |]
                |> JsonValue.Record

            let dataField =
                data |> deleteField "BankAccounts" |> addField "Bank" bankField

            Ok(root |> replaceField "Data" dataField |> setVersion 1m)
        | _ ->
            Error(InvalidStructure "BankAccounts should be present in the data")
    | _ -> Error(InvalidStructure "Data field should be on the root")
