namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Songs.Composition.DiscardSong

[<RequireQualifiedAccess>]
module DiscardSongCommand =
    /// Command to discard an unfinished song.
    let create unfinishedSongs =
        { Name = "discard song"
          Description = Command.waitDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.discardSongSelection
                        Generic.cancel
                        (fun (UnfinishedSong us, _, currentQuality) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song -> discardSong currentBand song |> Cli.Effect.apply
                | None -> ()

                Scene.World) }
