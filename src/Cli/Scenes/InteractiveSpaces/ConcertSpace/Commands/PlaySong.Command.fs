module Cli.Scenes.InteractiveSpaces.ConcertSpace.PlaySongCommand

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation
open Simulation.Concerts.Live.PlaySong

let textFromEnergy energy =
    match energy with
    | Energetic -> ConcertEnergyEnergetic
    | PerformEnergy.Normal -> ConcertEnergyNormal
    | Limited -> ConcertEnergyLow
    |> ConcertText
    |> I18n.translate

/// Command which simulates playing a song in a concert.
let rec create ongoingConcert concertScene =
    { Name = "play"
      Description =
        ConcertText ConcertCommandPlayDescription
        |> I18n.translate
      Handler =
        (fun _ ->
            promptForSong ongoingConcert
            |> concertScene
            |> Some) }

and private promptForSong ongoingConcert =
    let state = State.get ()
    let currentBand = Queries.Bands.currentBand state

    let finishedSongs =
        Queries.Repertoire.allFinishedSongsByBand state currentBand.Id

    if List.isEmpty finishedSongs then
        ConcertText ConcertNoSongsToPlay
        |> I18n.translate
        |> showMessage

        ongoingConcert
    else
        let selectedSong =
            showOptionalChoicePrompt
                (ConcertText ConcertSelectSongToPlay
                 |> I18n.translate)
                (CommonText CommonCancel |> I18n.translate)
                (fun (FinishedSong fs, _) ->
                    if Concert.Ongoing.hasPlayedSong ongoingConcert fs then
                        ConcertAlreadyPlayedSongWithPractice fs
                    else
                        ConcertSongNameWithPractice fs
                    |> ConcertText
                    |> I18n.translate)
                finishedSongs

        match selectedSong with
        | Some song -> promptForEnergy ongoingConcert song
        | None -> ongoingConcert

and private promptForEnergy ongoingConcert song =
    showChoicePrompt
        (ConcertText ConcertEnergyPrompt |> I18n.translate)
        textFromEnergy
        [ Energetic
          PerformEnergy.Normal
          Limited ]
    |> playSongWithProgressBar ongoingConcert song

and private playSongWithProgressBar ongoingConcert songWithQuality energy =
    let (FinishedSong song, _) = songWithQuality

    match energy with
    | Energetic -> ConcertPlaySongEnergeticEnergyDescription
    | PerformEnergy.Normal -> ConcertPlaySongNormalEnergyDescription
    | Limited -> ConcertPlaySongLimitedEnergyDescription
    |> ConcertText
    |> I18n.translate
    |> showMessage

    showProgressBar
        [ ConcertPlaySongProgressPlaying song
          |> ConcertText
          |> I18n.translate ]
        (song.Length.Minutes / 1<minute/second>)
        false

    let reactionText =
        match song.Practice with
        | p when p < 40<practice> -> ConcertPlaySongLowPracticeReaction energy
        | p when p < 80<practice> ->
            ConcertPlaySongMediumPracticeReaction energy
        | _ -> ConcertPlaySongHighPracticeReaction energy

    ConcertText reactionText
    |> I18n.translate
    |> showMessage

    playSong ongoingConcert songWithQuality energy
