module Duets.Data.Tests.SavegameMigrations

open FSharp.Data
open FSharp.Data.JsonExtensions
open FsUnit
open NUnit.Framework

open Data.Savegame.Migrations
open Duets.Common
open Duets.Data.Savegame.Migrations
open Duets.Data.Savegame.Types
open Duets.Entities

[<Test>]
let ``anything other than an object is not accepted as a root`` () =
    [ "[1,2,3]"; "3"; "45.4"; "\"test\""; "null"; "false" ]
    |> List.iter (fun inputData ->
        let result = applyMigrations inputData

        match result with
        | Error(MigrationError.InvalidStructure _) -> ()
        | res ->
            failwith $"Expected an error with invalid structure, got {res}")

[<Test>]
let ``versions higher than the current one result in an error`` () =
    let version = lastSavegameVersion + 1

    let input =
        $"""
{{
  "Version": {version},
  "Data": {{}}
}}
"""

    let result = applyMigrations input

    match result with
    | Error(MigrationError.InvalidVersion parsedVersion) ->
        parsedVersion |> should equal (version.ToString())
    | res -> failwith $"Expected an error with invalid version, got {res}"

[<Test>]
let ``versions that equal latest return original data`` () =
    let input =
        $"""
{{
  "Version": {lastSavegameVersion},
  "Data": {{}}
}}
"""

    let result = applyMigrations input

    match result with
    | Ok(result) -> result |> should equal input
    | res -> failwith $"Expected the original data, but got {res}"

[<Test>]
let ``savegames without version get migrated to the latest version`` () =
    let input = $"""
{{
    "Data": {{
        "BankAccounts": {{}},
        "Bands": {{}}
    }}
}}
"""

    let result = applyMigrations input

    match result with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        let version = json?Version.AsInteger()
        version |> should equal lastSavegameVersion
    | res -> failwith $"Expected non-error with JSON, got {res}"

(* --- Migration 1: AddLoanState --- *)

[<Test>]
let ``migration 1 restructures BankAccounts into Bank with default LoanState`` () =
    let input = """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = input |> JsonValue.Parse |> AddLoanState.migrate

    match result with
    | Ok(json) ->
        let loanState = json?Data?Bank?LoanState
        loanState?ActiveLoan |> should equal JsonValue.Null
        loanState?Reputation?Case.AsString() |> should equal "GoodStanding"
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 sets version to 1`` () =
    let input = """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = input |> JsonValue.Parse |> AddLoanState.migrate

    match result with
    | Ok(json) ->
        json?Version.AsInteger() |> should equal 1
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 removes BankAccounts and adds Bank`` () =
    let input = """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = input |> JsonValue.Parse |> AddLoanState.migrate

    match result with
    | Ok(json) ->
        json?Data.TryGetProperty("BankAccounts") |> should equal None
        json?Data.TryGetProperty("Bank") |> should not' (equal None)
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 preserves existing BankAccounts data under Bank.Accounts`` () =
    let input = """
{
    "Version": 0,
    "Data": {
        "BankAccounts": { "someKey": 42 }
    }
}
"""

    let result = input |> JsonValue.Parse |> AddLoanState.migrate

    match result with
    | Ok(json) ->
        let accounts = json?Data?Bank?Accounts
        accounts?someKey.AsInteger() |> should equal 42
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 errors when BankAccounts field is missing from Data`` () =
    let input = """
{
    "Version": 0,
    "Data": {}
}
"""

    let result = input |> JsonValue.Parse |> AddLoanState.migrate

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

(* --- Migration 2: MultiInstrumentMembers --- *)

let private date =
    """{"Day":1,"Season":{"Case":"Spring"},"Year":2025,"DayMoment":{"Case":"EarlyMorning"}}"""

let private bandJson id members pastMembers =
    $"""
["{id}", {{
    "Id": "{id}",
    "StartDate": {date},
    "OriginCity": {{ "Case": "NewYork" }},
    "Name": "Band",
    "Genre": "Jazz",
    "Fans": [],
    "Members": [{members}],
    "PastMembers": [{pastMembers}]
}}]
"""

let private currentMemberJson role =
    $"""{{ "CharacterId": "60cdedda-a32b-481f-b406-02dd80eccc20", "Role": {{ "Case": "{role}" }}, "Since": {date} }}"""

let private pastMemberJson role =
    $"""{{ "CharacterId": "60cdedda-a32b-481f-b406-02dd80eccc20", "Role": {{ "Case": "{role}" }}, "Period": [{date}, {date}] }}"""

let private characterBandId = "e40894f9-720f-4740-9639-d15e29427576"
let private simulatedBandId = "a1b2c3d4-720f-4740-9639-d15e29427576"

let private savegameWithBands =
    $"""
{{
    "Version": 1,
    "Data": {{
        "Bands": {{
            "Current": "{characterBandId}",
            "Character": [{bandJson characterBandId (currentMemberJson "Guitar") (pastMemberJson "Bass")}],
            "Simulated": [{bandJson simulatedBandId (currentMemberJson "Drums") ""}]
        }}
    }}
}}
"""

let private migratedBands () : Bands =
    match applyMigrations savegameWithBands with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        json?Version.AsInteger() |> should equal 2
        json?Data?Bands.ToString() |> Serializer.deserialize
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 turns the role of current members into a list of roles`` () =
    let bands = migratedBands ()

    let characterBand =
        bands.Character |> Map.values |> Seq.head

    characterBand.Members |> List.map _.Roles |> should equal [ [ Guitar ] ]

[<Test>]
let ``migration 2 turns the role of past members into a list of roles`` () =
    let bands = migratedBands ()

    let characterBand =
        bands.Character |> Map.values |> Seq.head

    characterBand.PastMembers |> List.map _.Roles |> should equal [ [ Bass ] ]

[<Test>]
let ``migration 2 migrates simulated bands`` () =
    let bands = migratedBands ()

    let simulatedBand =
        bands.Simulated |> Map.values |> Seq.head

    simulatedBand.Members |> List.map _.Roles |> should equal [ [ Drums ] ]

[<Test>]
let ``migration 2 errors when Bands field is missing from Data`` () =
    let input = """
{
    "Version": 1,
    "Data": {}
}
"""

    let result = input |> JsonValue.Parse |> MultiInstrumentMembers.migrate

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

[<Test>]
let ``savegames at version 1 only apply the migrations after version 1`` () =
    match applyMigrations savegameWithBands with
    | Ok _ -> ()
    | res -> failwith $"Expected migrated JSON, got {res}"
