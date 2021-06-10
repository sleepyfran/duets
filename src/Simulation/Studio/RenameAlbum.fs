module Simulation.Studio.RenameAlbum

open Entities

/// Renames an album to a given name, validating that the name is correct.
let renameAlbum band album name =
    Album.modifyName album name
    |> fun result ->
        match result with
        | Ok album -> Ok(album, AlbumRenamed(band, album))
        | Error error -> Error error
