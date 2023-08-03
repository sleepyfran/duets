namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation
open Duets.Simulation.Songs.Practice

[<RequireQualifiedAccess>]
module PracticeSongCommand =
    let private showPracticeSong practiceSongResult =
        match practiceSongResult with
        | SongImproved effects ->
            showProgressBarAsync
                [ Rehearsal.practiceSongProgressLosingTime
                  Rehearsal.practiceSongProgressTryingSoloOnceMore
                  Rehearsal.practiceSongProgressGivingUp ]
                2<second>

            Duets.Cli.Effect.applyMultiple effects
        | SongAlreadyImprovedToMax(Finished(song, _)) ->
            Rehearsal.practiceSongAlreadyImprovedToMax song.Name |> showMessage

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
                    practiceSong (State.get ()) currentBand song
                    |> showPracticeSong
                | None -> ()

                Scene.World) }
