namespace Simulation.Queries.Internal.Interactions

open Common
open Entities
open Simulation

module Studio =
    /// Returns all interactions available in the current studio room.
    let internal availableCurrently state studio room =
        let currentBand =
            Queries.Bands.currentBand state

        let finishedSongs =
            Queries.Songs.finishedByBand state currentBand.Id
            |> List.ofMapValues

        let unreleasedAlbums =
            Queries.Albums.unreleasedByBand state currentBand.Id
            |> List.ofMapValues

        let hasUnreleasedAlbums =
            not (List.isEmpty unreleasedAlbums)

        match room with
        | RoomType.MasteringRoom ->
            [ yield
                  StudioInteraction.CreateAlbum(studio, finishedSongs)
                  |> Interaction.Studio
              if hasUnreleasedAlbums then
                  yield
                      StudioInteraction.EditAlbumName unreleasedAlbums
                      |> Interaction.Studio

                  yield
                      StudioInteraction.ReleaseAlbum unreleasedAlbums
                      |> Interaction.Studio ]
        | _ -> []
