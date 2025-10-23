module Data.Savegame.Migrations.MigrateFromVersionless

open Duets.Data.Savegame.Types
open FSharp.Data

/// Migration that takes a root that contains an object without a version and
/// transforms it into an object that has version 0 and the current data in a
/// "Data" field.
let migrate (root: JsonValue) =
    match root with
    | JsonValue.Record values ->
        Ok(
            JsonValue.Record
                [| ("Version", JsonValue.Number(0m))
                   ("Data", JsonValue.Record(values)) |]
        )
    | _ -> Error(InvalidStructure("Root of savegame should be an object"))
