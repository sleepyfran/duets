namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module PracticeSongCommand =
    /// Command to practice a finished song.
    let create finishedSongs =
        { Name = "practice song"
          Description = Command.practiceSongDescription
          Handler =
            (fun _ ->
                let state = State.get ()
                let currentBand = Queries.Bands.currentBand state

                let sortedSongs =
                    finishedSongs
                    |> Seq.sortBy (fun (Finished(song, _)) -> song.Practice)

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.practiceSong
                        Generic.cancel
                        (fun (Finished(fs: Song, _)) ->
                            Rehearsal.practiceSongItemDescription
                                fs.Name
                                fs.Practice)
                        sortedSongs

                match selectedSong with
                | Some song ->
                    RehearsalRoomPracticeSong
                        {| Band = currentBand; Song = song |}
                    |> Effect.applyAction
                | None -> ()

                Scene.World) }
