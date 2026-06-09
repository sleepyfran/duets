module Duets.Data.Tests.SavegameMigrations

open FSharp.Data
open FSharp.Data.JsonExtensions
open FsUnit
open NUnit.Framework

open Duets.Data.Savegame.Migrations
open Duets.Data.Savegame.Types

let private jsonObject (fields: (string * JsonValue) list) =
    fields |> Array.ofList |> JsonValue.Record

let private jsonArray (values: JsonValue list) =
    values |> Array.ofList |> JsonValue.Array

let private jsonString value = JsonValue.String value

let private jsonNumber value = JsonValue.Number(decimal value)

let private tuple2 first second = jsonArray [ first; second ]

let private emptyObject = jsonObject []
let private emptyArray = jsonArray []

let private relationships byCharacterId =
    jsonObject [ "ByCharacterId", jsonArray byCharacterId ]

let private defaultMigrationData =
    Map.ofList
        [ 1, [ "BankAccounts", emptyObject ]
          2,
          [ "Characters", emptyArray
            "Relationships", relationships []
            "PeopleInCurrentPosition", emptyArray ] ]

let private withMigrationData migration data builder =
    builder |> Map.add migration data

let private buildData builder =
    builder
    |> Map.toSeq
    |> Seq.collect snd
    |> Seq.toArray
    |> JsonValue.Record

let private buildSavegame version builder =
    jsonObject [ "Version", jsonNumber version; "Data", buildData builder ]

let private buildSavegameString version builder =
    builder |> buildSavegame version |> string

let private buildVersionlessSavegameString builder =
    jsonObject [ "Data", buildData builder ] |> string

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
    let input = defaultMigrationData |> buildVersionlessSavegameString

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
    let input = defaultMigrationData |> buildSavegame 0

    let result = Data.Savegame.Migrations.AddLoanState.migrate input

    match result with
    | Ok(json) ->
        let loanState = json?Data?Bank?LoanState
        loanState?ActiveLoan |> should equal JsonValue.Null
        loanState?Reputation?Case.AsString() |> should equal "GoodStanding"
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 sets version to 1`` () =
    let input = defaultMigrationData |> buildSavegame 0

    let result = Data.Savegame.Migrations.AddLoanState.migrate input

    match result with
    | Ok(json) ->
        json?Version.AsInteger() |> should equal 1
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 removes BankAccounts and adds Bank`` () =
    let input = defaultMigrationData |> buildSavegame 0

    let result = Data.Savegame.Migrations.AddLoanState.migrate input

    match result with
    | Ok(json) ->
        json?Data.TryGetProperty("BankAccounts") |> should equal None
        json?Data.TryGetProperty("Bank") |> should not' (equal None)
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 preserves existing BankAccounts data under Bank.Accounts``
    ()
    =
    let input =
        defaultMigrationData
        |> withMigrationData 1 [ "BankAccounts", jsonObject [ "someKey", jsonNumber 42 ] ]
        |> buildSavegame 0

    let result = Data.Savegame.Migrations.AddLoanState.migrate input

    match result with
    | Ok(json) ->
        let accounts = json?Data?Bank?Accounts
        accounts?someKey.AsInteger() |> should equal 42
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 1 errors when BankAccounts field is missing from Data`` () =
    let input =
        defaultMigrationData
        |> withMigrationData 1 []
        |> buildSavegame 0

    let result = Data.Savegame.Migrations.AddLoanState.migrate input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

(* --- Migration 2: AddSocialFields --- *)

[<Test>]
let ``migration 2 adds empty Traits to characters without Traits`` () =
    let input =
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters",
              jsonArray
                  [ tuple2
                        (jsonString "character-1")
                        (jsonObject [ "Name", jsonString "Fran" ])
                    tuple2
                        (jsonString "character-2")
                        (jsonObject [ "Name", jsonString "Alex" ]) ]
              "Relationships", relationships []
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

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
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters",
              jsonArray
                  [ tuple2
                        (jsonString "character-1")
                        (jsonObject
                            [ "Name", jsonString "Fran"
                              "Traits", jsonArray [ jsonString "Warm" ] ]) ]
              "Relationships", relationships []
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

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
    let input = defaultMigrationData |> buildSavegame 1

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) -> json?Version.AsInteger() |> should equal 2
    | res -> failwith $"Expected migrated JSON, got {res}"

[<Test>]
let ``migration 2 errors when Characters field is missing from Data`` () =
    let input =
        defaultMigrationData
        |> withMigrationData
            2
            [ "Relationships", relationships []
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

[<Test>]
let ``migration 2 adds empty DiscoveredTraits to relationships without DiscoveredTraits``
    ()
    =
    let input =
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters", emptyArray
              "Relationships",
              relationships
                  [ tuple2
                        (jsonString "character-1")
                        (jsonObject [ "Level", jsonNumber 25 ])
                    tuple2
                        (jsonString "character-2")
                        (jsonObject [ "Level", jsonNumber 50 ]) ]
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

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
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters", emptyArray
              "Relationships",
              relationships
                  [ tuple2
                        (jsonString "character-1")
                        (jsonObject
                            [ "Level", jsonNumber 25
                              "DiscoveredTraits", jsonArray [ jsonString "Warm" ] ]) ]
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

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
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters", emptyArray
              "PeopleInCurrentPosition", emptyArray ]
        |> buildSavegame 1

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res -> failwith $"Expected InvalidStructure error, got {res}"

[<Test>]
let ``migration 2 resets PeopleInCurrentPosition regardless of what's there``
    ()
    =
    let input =
        defaultMigrationData
        |> withMigrationData
            2
            [ "Characters", emptyArray
              "Relationships",
              relationships
                  [ tuple2
                        (jsonString "character-1")
                        (jsonObject
                            [ "Level", jsonNumber 25
                              "DiscoveredTraits", jsonArray [ jsonString "Warm" ] ]) ]
              "PeopleInCurrentPosition",
              jsonArray [ jsonObject [ "Name", jsonString "Test" ] ] ]
        |> buildSavegame 1

    let result = Data.Savegame.Migrations.AddSocialFields.migrate input

    match result with
    | Ok(json) ->
        let npcs = json?Data?PeopleInCurrentPosition.AsArray()
        npcs.Length |> should equal 0
    | res -> failwith $"Expected migrated JSON, got {res}"
