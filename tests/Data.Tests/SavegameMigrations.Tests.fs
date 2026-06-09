module Duets.Data.Tests.SavegameMigrations

open FSharp.Data
open FSharp.Data.JsonExtensions
open FsUnit
open NUnit.Framework

open Duets.Data.Savegame.Migrations
open Duets.Data.Savegame.Types

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
    let input =
        $"""
{{
    "Data": {{
        "BankAccounts": {{}}
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
let ``migration 1 restructures BankAccounts into Bank with default LoanState``
    ()
    =
    let input =
        """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = applyMigrations input

    match result with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        let loanState = json?Data?Bank?LoanState
        loanState?ActiveLoan |> should equal JsonValue.Null
        loanState?Reputation?Case.AsString() |> should equal "GoodStanding"
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 sets version to 1`` () =
    let input =
        """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = applyMigrations input

    match result with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        json?Version.AsInteger() |> should equal 1
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 removes BankAccounts and adds Bank`` () =
    let input =
        """
{
    "Version": 0,
    "Data": {
        "BankAccounts": {}
    }
}
"""

    let result = applyMigrations input

    match result with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        json?Data.TryGetProperty("BankAccounts") |> should equal None
        json?Data.TryGetProperty("Bank") |> should not' (equal None)
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 preserves existing BankAccounts data under Bank.Accounts``
    ()
    =
    let input =
        """
{
    "Version": 0,
    "Data": {
        "BankAccounts": { "someKey": 42 }
    }
}
"""

    let result = applyMigrations input

    match result with
    | Ok(json) ->
        let json = JsonValue.Parse(json)
        let accounts = json?Data?Bank?Accounts
        accounts?someKey.AsInteger() |> should equal 42
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 errors when BankAccounts field is missing from Data`` () =
    let input =
        """
{
    "Version": 0,
    "Data": {}
}
"""

    let result = applyMigrations input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

(* --- Migration 2: AddSocialFields --- *)

[<Test>]
let ``migration 2 adds empty Traits to characters without Traits`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [
            ["character-1", { "Name": "Fran" }],
            ["character-2", { "Name": "Alex" }]
        ],
        "Relationships": {
            "ByCharacterId": []
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let characters = json?Data?Characters.AsArray()
        let firstCharacter = characters[0].AsArray()[1]
        let secondCharacter = characters[1].AsArray()[1]

        firstCharacter?Traits.AsArray().Length |> should equal 0
        secondCharacter?Traits.AsArray().Length |> should equal 0
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 preserves existing Traits on characters`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [
            ["character-1", { "Name": "Fran", "Traits": ["Warm"] }]
        ],
        "Relationships": {
            "ByCharacterId": []
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let characters = json?Data?Characters.AsArray()
        let character = characters[0].AsArray()[1]
        let traits = character?Traits.AsArray()

        traits.Length |> should equal 1
        traits[0].AsString() |> should equal "Warm"
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 sets version to 2`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [],
        "Relationships": {
            "ByCharacterId": []
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) -> json?Version.AsInteger() |> should equal 2
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 errors when Characters field is missing from Data`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Relationships": {
            "ByCharacterId": []
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

[<Test>]
let ``migration 2 adds empty DiscoveredTraits to relationships without DiscoveredTraits``
    ()
    =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [],
        "Relationships": {
            "ByCharacterId": [
                ["character-1", { "Level": 25 }],
                ["character-2", { "Level": 50 }]
            ]
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let relationships = json?Data?Relationships?ByCharacterId.AsArray()
        let firstRelationship = relationships[0].AsArray()[1]
        let secondRelationship = relationships[1].AsArray()[1]

        firstRelationship?DiscoveredTraits.AsArray().Length |> should equal 0
        secondRelationship?DiscoveredTraits.AsArray().Length |> should equal 0
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 preserves existing DiscoveredTraits on relationships`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [],
        "Relationships": {
            "ByCharacterId": [
                ["character-1", { "Level": 25, "DiscoveredTraits": ["Warm"] }]
            ]
        }
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let relationships = json?Data?Relationships?ByCharacterId.AsArray()
        let relationship = relationships[0].AsArray()[1]
        let discoveredTraits = relationship?DiscoveredTraits.AsArray()

        discoveredTraits.Length |> should equal 1
        discoveredTraits[0].AsString() |> should equal "Warm"
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 errors when Relationships field is missing from Data`` () =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": []
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

[<Test>]
let ``migration 2 resets PeopleInCurrentPosition regardless of what's there``
    ()
    =
    let input =
        JsonValue.Parse
            """
{
    "Version": 1,
    "Data": {
        "Characters": [],
        "Relationships": {
            "ByCharacterId": [
                ["character-1", { "Level": 25, "DiscoveredTraits": ["Warm"] }]
            ]
        },
        "PeopleInCurrentPosition": [
            { "Name": "Test" }
        ]
    }
}
"""

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let npcs = json?Data?PeopleInCurrentPosition.AsArray()
        npcs.Length |> should equal 0
    | res -> failwith $"Expected migrated JSON, got {res}"
