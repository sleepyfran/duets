namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Songs.Composition.FinishSong

[<RequireQualifiedAccess>]
module FinishSongCommand =
    /// Command to finish an unfinished song.
    let create unfinishedSongs =
        { Name = "finish song"
          Description = Command.finishSongDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.finishSongSelection
                        Generic.cancel
                        (fun (UnfinishedSong us, _, currentQuality) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song -> finishSong currentBand song |> Cli.Effect.apply
                | None -> ()

                Scene.World) }
