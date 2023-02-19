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
        | SongAlreadyImprovedToMax (FinishedSong song, _) ->
            Rehearsal.practiceSongAlreadyImprovedToMax song.Name |> showMessage

    /// Command to practice a finished song.
    let create finishedSongs =
        { Name = "practice song"
          Description = Command.practiceSongDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand = Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.practiceSong
                        Generic.cancel
                        (fun (FinishedSong fs, _) ->
                            Rehearsal.practiceSongItemDescription
                                fs.Name
                                fs.Practice)
                        finishedSongs

                match selectedSong with
                | Some song ->
                    practiceSong (State.get ()) currentBand song
                    |> showPracticeSong
                | None -> ()

                Scene.World) }