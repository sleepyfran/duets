namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common
open Duets.Entities
open Duets.Simulation

module Studio =
    /// Returns all interactions available in the current studio room.
    let internal interactions state studio studioQuality =
        let currentBand = Queries.Bands.currentBand state

        let finishedSongs =
            Queries.Songs.finishedNonRecordedByBand state currentBand.Id
            |> List.ofMapValues
            |> List.map Song.Finished.fromFinishedWithStatus

        let unreleasedAlbums =
            Queries.Albums.unreleasedByBand state currentBand.Id
            |> List.ofMapValues

        let hasUnreleasedAlbums = not (List.isEmpty unreleasedAlbums)

        [ StudioInteraction.CreateAlbum(studio, finishedSongs)
          |> Interaction.Studio
          if hasUnreleasedAlbums then
              yield!
                  [ StudioInteraction.AddSongToAlbum(
                        studio,
                        unreleasedAlbums,
                        finishedSongs
                    )
                    |> Interaction.Studio

                    StudioInteraction.EditAlbumName unreleasedAlbums
                    |> Interaction.Studio

                    StudioInteraction.ListUnreleasedAlbums unreleasedAlbums
                    |> Interaction.Studio

                    StudioInteraction.ReleaseAlbum unreleasedAlbums
                    |> Interaction.Studio ] ]
