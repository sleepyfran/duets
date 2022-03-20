module Cli.Scenes.InteractiveSpaces.ConcertSpace.PlaySongCommand

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Entities
open Simulation
open Simulation.Concerts.Live.PlaySong

let textFromEnergy energy =
    match energy with
    | Energetic -> ConcertEnergyEnergetic
    | PerformEnergy.Normal -> ConcertEnergyNormal
    | Limited -> ConcertEnergyLow
    |> ConcertText
    |> I18n.translate

let private promptForEnergy ongoingConcert song =
    showChoicePrompt
        (ConcertText ConcertEnergyPrompt |> I18n.translate)
        textFromEnergy
        [ Energetic
          PerformEnergy.Normal
          Limited ]
    |> playSong ongoingConcert song

let private promptForSong ongoingConcert =
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

/// Command which simulates playing a song in a concert.
let create ongoingConcert concertScene =
    { Name = "play"
      Description =
          ConcertText ConcertCommandPlayDescription
          |> I18n.translate
      Handler =
          (fun _ ->
              promptForSong ongoingConcert
              |> concertScene
              |> Some) }
