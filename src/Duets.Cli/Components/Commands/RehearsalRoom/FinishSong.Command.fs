namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Songs.Composition.FinishSong

[<RequireQualifiedAccess>]
module FinishSongCommand =
    /// Command to finish an unfinished song.
    let create unfinishedSongs =
        { Name = "finish song"
          Description = Command.finishSongDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand = Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.finishSongSelection
                        Generic.cancel
                        (fun (Unfinished(us: Song, _, currentQuality)) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song ->
                    finishSong (State.get ()) currentBand song
                    |> Duets.Cli.Effect.apply
                | None -> ()

                Scene.World) }
