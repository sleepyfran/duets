module Duets.Simulation.Studio.RenameAlbum

open Duets.Entities

/// Renames an album to a given name, validating that the name is correct.
let renameAlbum band album name =
    Album.Unreleased.modifyName album name
    |> fun album -> AlbumUpdated(band, album)
