namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Songs.Composition.DiscardSong

[<RequireQualifiedAccess>]
module DiscardSongCommand =
    /// Command to discard an unfinished song.
    let create unfinishedSongs =
        { Name = "discard song"
          Description = Command.waitDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand = Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.discardSongSelection
                        Generic.cancel
                        (fun (Unfinished(us: Song, _, currentQuality)) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song ->
                    RehearsalRoomDiscardSong
                        {| Band = currentBand; Song = song |}
                    |> Duets.Cli.Effect.applyAction
                | None -> ()

                Scene.World) }
