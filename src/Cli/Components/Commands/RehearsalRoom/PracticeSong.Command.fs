namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation
open Simulation.Songs.Practice

[<RequireQualifiedAccess>]
module PracticeSongCommand =
    let private showPracticeSong practiceSongResult =
        match practiceSongResult with
        | SongImproved effect ->
            showProgressBarAsync
                [ Rehearsal.practiceSongProgressLosingTime
                  Rehearsal.practiceSongProgressTryingSoloOnceMore
                  Rehearsal.practiceSongProgressGivingUp ]
                2<second>

            Cli.Effect.apply effect
        | SongAlreadyImprovedToMax (FinishedSong song, _) ->
            Rehearsal.practiceSongAlreadyImprovedToMax song.Name
            |> showMessage

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
                | Some song -> practiceSong currentBand song |> showPracticeSong
                | None -> ()

                Scene.World) }
