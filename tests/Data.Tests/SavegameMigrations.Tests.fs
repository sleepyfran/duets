module Duets.Data.Tests.SavegameMigrations

open Duets.Common
open Duets.Data.Savegame.Migrations
open Duets.Data.Savegame.Types
open Duets.Entities
open FsUnit
open NUnit.Framework

type private VersionlessSavegame = { Data: State }

let private buildSavegame version =
    { Version = version; Data = State.empty } |> Serializer.serializeBinary

let private buildVersionlessSavegame () =
    { Data = State.empty } |> Serializer.serializeBinary

[<Test>]
let ``malformed MessagePack is not accepted`` () =
    let result = applyMigrations [| 0x01uy |]

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res ->
        failwith $"Expected an invalid structure error, got {res}"

[<Test>]
let ``savegames without version are not accepted`` () =
    let result = buildVersionlessSavegame () |> applyMigrations

    match result with
    | Error(MigrationError.InvalidStructure _) -> ()
    | res ->
        failwith $"Expected an invalid structure error, got {res}"

[<Test>]
let ``versions lower than the first MessagePack version result in an error`` () =
    let version = firstSavegameVersion - 1
    let result = buildSavegame version |> applyMigrations

    match result with
    | Error(MigrationError.InvalidVersion parsedVersion) ->
        parsedVersion |> should equal (version.ToString())
    | res -> failwith $"Expected an invalid version error, got {res}"

[<Test>]
let ``versions higher than the current one result in an error`` () =
    let version = lastSavegameVersion + 1
    let result = buildSavegame version |> applyMigrations

    match result with
    | Error(MigrationError.InvalidVersion parsedVersion) ->
        parsedVersion |> should equal (version.ToString())
    | res -> failwith $"Expected an invalid version error, got {res}"

[<Test>]
let ``versions that equal latest return original data`` () =
    let input = buildSavegame lastSavegameVersion
    let result = applyMigrations input

    match result with
    | Ok(result) -> result |> should equal input
    | res -> failwith $"Expected the original data, got {res}"

[<Test>]
let ``latest savegame payload round trips through MessagePack`` () =
    let input = buildSavegame lastSavegameVersion

    match Serializer.tryDeserializeBinary<SavegameContents> input with
    | Some savegame ->
        savegame.Version |> should equal lastSavegameVersion
        savegame.Data |> should equal State.empty
    | None -> failwith "Expected savegame to deserialize"
