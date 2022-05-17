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
                [ I18n.translate (
                      RehearsalSpaceText PracticeSongProgressLosingTime
                  )
                  I18n.translate (
                      RehearsalSpaceText PracticeSongProgressTryingSoloOnceMore
                  )
                  I18n.translate (
                      RehearsalSpaceText PracticeSongProgressGivingUp
                  ) ]
                2<second>

            Cli.Effect.apply effect
        | SongAlreadyImprovedToMax (FinishedSong song, _) ->
            PracticeSongAlreadyImprovedToMax song.Name
            |> RehearsalSpaceText
            |> I18n.translate
            |> showMessage

    /// Command to practice a finished song.
    let create finishedSongs =
        { Name = "practice song"
          Description =
            I18n.translate (CommandText CommandPracticeSongDescription)
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        (RehearsalSpaceText PracticeSong |> I18n.translate)
                        (CommonText CommonCancel |> I18n.translate)
                        (fun (FinishedSong fs, _) ->
                            PracticeSongItemDescription(fs.Name, fs.Practice)
                            |> RehearsalSpaceText
                            |> I18n.translate)
                        finishedSongs

                match selectedSong with
                | Some song -> practiceSong currentBand song |> showPracticeSong
                | None -> ()

                Scene.World) }
