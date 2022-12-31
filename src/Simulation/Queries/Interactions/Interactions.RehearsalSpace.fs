namespace Simulation.Queries.Internal.Interactions


open Common
open Entities
open Simulation

module RehearsalSpace =
    /// Returns all interactions available in the current rehearsal room.
    let internal availableCurrently state =
        let currentBand = Queries.Bands.currentBand state

        let unfinishedSongs =
            Queries.Songs.unfinishedByBand state currentBand.Id
            |> List.ofMapValues

        let hasUnfinishedSongs = unfinishedSongs |> (not << List.isEmpty)

        let finishedSongs =
            Queries.Repertoire.allFinishedSongsByBand state currentBand.Id

        let hasFinishedSongs = finishedSongs |> (not << List.isEmpty)

        let bandMembersWithoutPlayableCharacter =
            Queries.Bands.currentBandMembersWithoutPlayableCharacter state

        let currentBandMembers = Queries.Bands.currentBandMembers state

        let pastBandMembers = Queries.Bands.pastBandMembers state

        let hasMembersAsideOfPlayableCharacter =
            bandMembersWithoutPlayableCharacter |> (not << List.isEmpty)

        [ yield Interaction.Rehearsal RehearsalInteraction.ComposeNewSong
          if hasUnfinishedSongs then
              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.DiscardSong unfinishedSongs
                  )

              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.FinishSong unfinishedSongs
                  )

              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.ImproveSong unfinishedSongs
                  )

              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.DiscardSong unfinishedSongs
                  )

          if hasFinishedSongs then
              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.PracticeSong finishedSongs
                  )

          if hasUnfinishedSongs || hasFinishedSongs then
              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.ListSongs(
                          unfinishedSongs,
                          finishedSongs
                      )
                  )

          yield Interaction.Rehearsal RehearsalInteraction.HireMember
          if hasMembersAsideOfPlayableCharacter then
              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.FireMember
                          bandMembersWithoutPlayableCharacter
                  )

              yield
                  Interaction.Rehearsal(
                      RehearsalInteraction.ListMembers(
                          currentBandMembers,
                          pastBandMembers
                      )
                  ) ]
