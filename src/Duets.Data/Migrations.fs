module rec Duets.Data.Savegame.Migrations

open Data.Savegame.Migrations
open Data.Savegame.Migrations.Mandatory
open Duets.Common
open Duets.Data.Savegame.Types
open FSharp.Data

/// Array of migrations that can be applied. Should have migrations from the
/// very first supported version in incremental steps up until the last supported
/// version.
let private migrations =
    [ MigrateFromVersionless.migrate
      AddLoanState.migrate
      MultiInstrumentMembers.migrate
      GenericSoloEvents.migrate ]

/// Migrations that run on every savegame regardless of its version, after any
/// versioned migration. They must leave the savegame as-is if there's nothing
/// to do.
let private mandatoryMigrations = [ AddMissingGenreMarkets.migrate ]

/// Last version of savegame data that has a migration associated. Should
/// always be the last index of the migrations array, since that's how we
/// compute which migrations need to be performed.
let lastSavegameVersion = migrations.Length - 1

/// Attempts to parse the given JSON file and compares the version in the data
/// with the last available one, applying any migrations needed to bring the
/// savegame data up to date. If any migration fails, returns an error with
/// details about what went wrong.
let applyMigrations (currentData: string) : Result<string, MigrationError> =
    let data = JsonValue.Parse(currentData)

    match data with
    | JsonValue.Record _ -> applyMigrations' currentData data
    | _ ->
        Error(
            InvalidStructure("Root of the save-game data should be an object")
        )

let private applyMigrations' originalData root =
    let currentVersion = root.TryGetProperty("Version")

    match currentVersion with
    | Some(JsonValue.Number version) ->
        let version = Math.roundDecimalToNearest version

        (* Already at the last version, so only mandatory migrations apply. *)
        if version = lastSavegameVersion then
            applyMandatoryMigrations' originalData root
        else if version > lastSavegameVersion then
            Error(InvalidVersion(version.ToString()))
        else
            applyMigrationsFromVersion (version + 1) root
    | Some value -> Error(InvalidVersion(value.ToString()))
    | _ ->
        // No version means we haven't even performed the first migration, start
        // from the very beginning.
        applyMigrationsFromVersion 0 root

let private applyMigrationsFromVersion firstMigration root =
    let applicableMigrations =
        (migrations |> List.skip firstMigration) @ mandatoryMigrations

    let result = applyAllMigrations applicableMigrations root

    match result with
    | Ok(root) -> Ok(root.ToString())
    | Error(error) -> Error(error)

let private applyMandatoryMigrations' originalData root =
    let result = applyAllMigrations mandatoryMigrations root
    match result with
    | Ok(migrated) when migrated = root -> Ok(originalData)
    | Ok(migrated) -> Ok(migrated.ToString())
    | Error(error) -> Error(error)

let private applyAllMigrations migrations root =
    match migrations with
    | [] -> Ok(root)
    | migration :: tail ->
        match migration root with
        | Ok(root) -> applyAllMigrations tail root
        | err -> err
