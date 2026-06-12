module rec Duets.Data.Savegame.Migrations

open Duets.Data.Savegame.Types
open Nerdbank.MessagePack
open System

type private Migration = byte array -> Result<byte array, MigrationError>

/// The first savegame version that uses MessagePack.
let firstSavegameVersion = 0

/// Array of migrations that can be applied. Should have migrations from the
/// first MessagePack version in incremental steps up until the last supported
/// version.
let private migrations: Migration list = []

/// Last version of savegame data that has a migration associated.
let lastSavegameVersion = firstSavegameVersion + migrations.Length

let private tryReadVersion (data: byte array) =
    try
        let bytes = ReadOnlyMemory<byte>(data)
        let context = SerializationContext()
        let mutable reader = MessagePackReader(bytes)
        let mutable remainingFields = reader.ReadMapHeader()
        let mutable version = None

        while remainingFields > 0 && Option.isNone version do
            let key = reader.ReadString()

            if key = "Version" then
                version <- Some(reader.ReadInt32())
            else
                reader.Skip(context)

            remainingFields <- remainingFields - 1

        version
    with _ ->
        None

/// Attempts to parse the given MessagePack data and compares the version in
/// the data with the last available one, applying any migrations needed to
/// bring the savegame data up to date. If any migration fails, returns an error
/// with details about what went wrong.
let applyMigrations
    (currentData: byte array)
    : Result<byte array, MigrationError> =
    match currentData |> tryReadVersion with
    | Some version ->
        if version < firstSavegameVersion then
            Error(InvalidVersion(version.ToString()))
        elif version = lastSavegameVersion then
            Ok(currentData)
        elif version > lastSavegameVersion then
            Error(InvalidVersion(version.ToString()))
        else
            applyMigrationsFromVersion' version currentData
    | None ->
        Error(
            InvalidStructure(
                "Savegame data should be a MessagePack object with a Version field"
            )
        )

let private applyMigrationsFromVersion' originVersion data =
    let migrationsToSkip = originVersion - firstSavegameVersion
    let applicableMigrations = migrations |> List.skip migrationsToSkip
    applyAllMigrations applicableMigrations data

let private applyAllMigrations migrations data =
    match migrations with
    | [] -> Ok(data)
    | migration :: tail ->
        match migration data with
        | Ok(data) -> applyAllMigrations tail data
        | err -> err
