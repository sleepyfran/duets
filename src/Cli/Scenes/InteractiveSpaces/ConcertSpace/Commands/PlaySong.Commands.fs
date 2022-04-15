namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.Scenes.InteractiveSpaces.ConcertSpace
open Cli.Text
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module PlaySongCommands =
    let private textFromEnergy energy =
        match energy with
        | Energetic -> ConcertEnergyEnergetic
        | PerformEnergy.Normal -> ConcertEnergyNormal
        | Limited -> ConcertEnergyLow
        |> ConcertText
        |> I18n.translate

    let private promptForSong ongoingConcert =
        let state = State.get ()
        let currentBand = Queries.Bands.currentBand state

        let finishedSongs =
            Queries.Repertoire.allFinishedSongsByBand state currentBand.Id

        if List.isEmpty finishedSongs then
            ConcertText ConcertNoSongsToPlay
            |> I18n.translate
            |> showMessage

            None
        else
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

    let private promptForEnergy () =
        showChoicePrompt
            (ConcertText ConcertEnergyPrompt |> I18n.translate)
            textFromEnergy
            [ Energetic
              PerformEnergy.Normal
              Limited ]

    let private showResultWithProgressbar result points songWithQuality energy =
        let (FinishedSong song, _) = songWithQuality

        match result with
        | RepeatedSong ->
            ConcertPlaySongRepeatedSongReaction song
            |> ConcertText
            |> I18n.translate
            |> showMessage
        | _ ->
            match energy with
            | Energetic -> ConcertPlaySongEnergeticEnergyDescription
            | PerformEnergy.Normal -> ConcertPlaySongNormalEnergyDescription
            | Limited -> ConcertPlaySongLimitedEnergyDescription
            |> ConcertText
            |> I18n.translate
            |> showMessage

        showProgressBarSync
            [ ConcertPlaySongProgressPlaying song
              |> ConcertText
              |> I18n.translate ]
            (song.Length.Minutes / 1<minute/second>)

        match result with
        | RepeatedSong -> ConcertPlaySongRepeatedTipReaction points
        | LowPracticePerformance ->
            ConcertPlaySongLowPracticeReaction(energy, points)
        | NormalPracticePerformance ->
            ConcertPlaySongMediumPracticeReaction(energy, points)
        | HighPracticePerformance ->
            ConcertPlaySongHighPracticeReaction(energy, points)
        |> ConcertText
        |> I18n.translate
        |> showMessage

    /// Command which simulates playing a song in a concert.
    let createPlaySong ongoingConcert concertScene =
        { Name = "play song"
          Description =
              ConcertText ConcertCommandPlayDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  promptForSong ongoingConcert
                  |> Option.bind
                      (fun song ->
                          let energy = promptForEnergy ()
                          let response = playSong ongoingConcert song energy

                          showResultWithProgressbar
                              response.Result
                              response.Points
                              song
                              energy

                          Some response.OngoingConcert)
                  |> Option.defaultValue ongoingConcert
                  |> concertScene) }

    // Command which simulates dedicating a song and then playing it in a concert.
    let createDedicateSong ongoingConcert concertScene =
        { Name = "dedicate song"
          Description =
              ConcertText ConcertCommandDedicateSongDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  promptForSong ongoingConcert
                  |> Option.bind
                      (fun song ->
                          let energy = promptForEnergy ()
                          Components.showSpeechProgress ()
                          let response = dedicateSong ongoingConcert song energy

                          match response.Result with
                          | Dedicated playResult ->
                              showResultWithProgressbar
                                  playResult
                                  response.Points
                                  song
                                  energy
                          | TooManyDedications ->
                              ConcertTooManyDedications
                              |> ConcertText
                              |> I18n.translate
                              |> showMessage

                          Some response.OngoingConcert)
                  |> Option.defaultValue ongoingConcert
                  |> concertScene) }
