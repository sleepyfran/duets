namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
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

        let currentBand =
            Queries.Bands.currentBand state

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

    let private showResultWithProgressbar response songWithQuality energy =
        let (FinishedSong song, _) = songWithQuality

        match response.Result with
        | TooManyRepetitionsPenalized
        | TooManyRepetitionsNotDone ->
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

        match response.Result with
        | LowPerformance
        | AveragePerformance ->
            ConcertPlaySongLowPracticeReaction(energy, response.Points)
        | GoodPerformance ->
            ConcertPlaySongMediumPracticeReaction(energy, response.Points)
        | GreatPerformance ->
            ConcertPlaySongHighPracticeReaction(energy, response.Points)
        | _ -> ConcertPlaySongRepeatedTipReaction response.Points
        |> ConcertText
        |> I18n.translate
        |> showMessage

    /// Command which simulates playing a song in a concert.
    let createPlaySong ongoingConcert =
        { Name = "play song"
          Description =
            ConcertText ConcertCommandPlayDescription
            |> I18n.translate
          Handler =
            (fun _ ->
                promptForSong ongoingConcert
                |> Option.bind (fun song ->
                    let energy = promptForEnergy ()

                    let response =
                        playSong (State.get ()) ongoingConcert song energy

                    response.Effects |> Cli.Effect.applyMultiple

                    showResultWithProgressbar response song energy

                    Some response.OngoingConcert)
                |> Option.defaultValue ongoingConcert
                |> Situations.inConcert
                |> Cli.Effect.apply

                Scene.World) }

    // Command which simulates dedicating a song and then playing it in a concert.
    let createDedicateSong ongoingConcert =
        { Name = "dedicate song"
          Description =
            ConcertText ConcertCommandDedicateSongDescription
            |> I18n.translate
          Handler =
            (fun _ ->
                promptForSong ongoingConcert
                |> Option.bind (fun song ->
                    let energy = promptForEnergy ()
                    Common.showSpeechProgress ()

                    let response =
                        dedicateSong (State.get ()) ongoingConcert song energy

                    response.Effects |> Cli.Effect.applyMultiple

                    match response.Result with
                    | TooManyRepetitionsPenalized
                    | TooManyRepetitionsNotDone ->
                        ConcertTooManyDedications
                        |> ConcertText
                        |> I18n.translate
                        |> showMessage
                    | _ -> showResultWithProgressbar response song energy

                    Some response.OngoingConcert)
                |> Option.defaultValue ongoingConcert
                |> Situations.inConcert
                |> Cli.Effect.apply

                Scene.World) }
