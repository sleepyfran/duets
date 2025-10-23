module Duets.Data.Savegame.Types

open Duets.Entities

/// Contents of the savegame file, which contains a version for migration
/// purposes and the actual data.
type SavegameContents = { Version: uint; Data: State }

/// Errors that can occur during the application of migrations.
type MigrationError =
    | InvalidStructure of message: string
    | InvalidVersion of parsedVersion: string
