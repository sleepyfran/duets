module Cli.Scenes.InteractiveSpaces.RehearsalRoom.PracticeSong

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Practice

let rec practiceSongSubScene () = promptForSong ()

and private promptForSong () =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let songs = Repertoire.allFinishedSongsByBand state currentBand.Id

    let selectedSong =
        showOptionalChoicePrompt
            (RehearsalSpaceText DiscardSongSelection
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (FinishedSong fs, _) ->
                PracticeSongItemDescription(fs.Name, fs.Practice)
                |> RehearsalSpaceText
                |> I18n.translate)
            songs

    match selectedSong with
    | Some song -> showPracticeSong currentBand song
    | None -> ()

    Scene.World

and private showPracticeSong band song =
    let effect = practiceSong band song

    match effect with
    | SongImproved effect ->
        showProgressBar
            [ I18n.translate (RehearsalSpaceText PracticeSongProgressLosingTime)
              I18n.translate (
                  RehearsalSpaceText PracticeSongProgressTryingSoloOnceMore
              )
              I18n.translate (RehearsalSpaceText PracticeSongProgressGivingUp) ]
            2<second>
            true

        Cli.Effect.apply effect
    | SongAlreadyImprovedToMax (FinishedSong song, _) ->
        PracticeSongAlreadyImprovedToMax song.Name
        |> RehearsalSpaceText
        |> I18n.translate
        |> showMessage
