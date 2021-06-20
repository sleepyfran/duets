namespace State

module Albums =
    open Aether
    open Entities

    let private applyToUnreleased map bandId op =
        let unreleasedAlbumsLens =
            Lenses.FromState.Albums.unreleasedByBand_ bandId

        map (Optic.map unreleasedAlbumsLens op)

    let private applyToReleased map bandId op =
        let releasedAlbumsLens =
            Lenses.FromState.Albums.releasedByBand_ bandId

        map (Optic.map releasedAlbumsLens op)

    let addUnreleased map (band: Band) unreleasedAlbum =
        let (UnreleasedAlbum album) = unreleasedAlbum
        let addUnreleasedAlbum = Map.add album.Id unreleasedAlbum
        applyToUnreleased map band.Id addUnreleasedAlbum

    let addReleased map (band: Band) releasedAlbum =
        let album = releasedAlbum.Album
        let addReleasedAlbum = Map.add album.Id releasedAlbum
        applyToReleased map band.Id addReleasedAlbum

    let removeUnreleased map (band: Band) albumId =
        let removeUnreleasedAlbum = Map.remove albumId

        applyToUnreleased map band.Id removeUnreleasedAlbum

    let removeReleased map (band: Band) albumId =
        let removeReleasedAlbum = Map.remove albumId

        applyToReleased map band.Id removeReleasedAlbum
