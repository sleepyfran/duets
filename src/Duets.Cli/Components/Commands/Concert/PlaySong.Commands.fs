namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module PlaySongCommands =
    let private textFromEnergy energy =
        match energy with
        | Energetic -> Concert.energyEnergetic
        | PerformEnergy.Normal -> Concert.energyNormal
        | Limited -> Concert.energyLow

    let private promptForSong ongoingConcert =
        let state = State.get ()

        let currentBand = Queries.Bands.currentBand state

        let finishedSongs =
            Queries.Songs.finishedByBand state currentBand.Id
            |> List.ofMapValues
            |> List.map (fun finishedSong ->
                let song = Song.fromFinished finishedSong
                finishedSong, Concert.Ongoing.hasPlayedSong ongoingConcert song)
            |> List.sortBy snd

        if List.isEmpty finishedSongs then
            Concert.noSongsToPlay |> showMessage

            None
        else
            showOptionalChoicePrompt
                Concert.selectSongToPlay
                Generic.cancel
                (fun (finishedSong, alreadyPlayed) ->
                    let song = Song.fromFinished finishedSong

                    if alreadyPlayed then
                        Concert.alreadyPlayedSongWithPractice song
                    else
                        Concert.songNameWithPractice song)
                finishedSongs

    let private promptForEnergy () =
        showChoicePrompt
            Concert.energyPrompt
            textFromEnergy
            [ Energetic; PerformEnergy.Normal; Limited ]

    let private showResultWithProgressbar response songWithQuality energy =
        let (Finished(song, _)) = songWithQuality

        match response.Result with
        | TooManyRepetitionsPenalized
        | TooManyRepetitionsNotDone ->
            Concert.playSongRepeatedSongReaction song |> showMessage
        | _ ->
            match energy with
            | Energetic -> Concert.playSongEnergeticEnergyDescription
            | PerformEnergy.Normal -> Concert.playSongNormalEnergyDescription
            | Limited -> Concert.playSongLimitedEnergyDescription
            |> showMessage

        showProgressBarSync
            [ Concert.playSongProgressPlaying song ]
            (song.Length.Minutes / 1<minute / second>)

        match response.Result with
        | LowPerformance reasons
        | AveragePerformance reasons ->
            Concert.playSongLowPerformanceReaction
                energy
                reasons
                response.Points
        | GoodPerformance reasons ->
            Concert.playSongMediumPerformanceReaction reasons response.Points
        | GreatPerformance ->
            Concert.playSongHighPerformanceReaction energy response.Points
        | _ -> Concert.playSongRepeatedTipReaction response.Points
        |> showMessage

    /// Command which simulates playing a song in a concert.
    let createPlaySong ongoingConcert =
        { Name = "play song"
          Description = Command.playDescription
          Handler =
            (fun _ ->
                let selectedSong = promptForSong ongoingConcert

                match selectedSong with
                | Some(song, _) ->
                    let energy = promptForEnergy ()

                    let response =
                        playSong (State.get ()) ongoingConcert song energy

                    showResultWithProgressbar response song energy

                    response.Effects |> Duets.Cli.Effect.applyMultiple
                | None -> ()

                Scene.World) }

    // Command which simulates dedicating a song and then playing it in a concert.
    let createDedicateSong ongoingConcert =
        { Name = "dedicate song"
          Description = Command.dedicateSongDescription
          Handler =
            (fun _ ->
                let selectedSong = promptForSong ongoingConcert

                match selectedSong with
                | Some(song, _) ->
                    let energy = promptForEnergy ()
                    Concert.showSpeechProgress ()

                    let response =
                        dedicateSong (State.get ()) ongoingConcert song energy

                    match response.Result with
                    | TooManyRepetitionsPenalized
                    | TooManyRepetitionsNotDone ->
                        Concert.tooManyDedications |> showMessage
                    | _ -> showResultWithProgressbar response song energy

                    response.Effects |> Duets.Cli.Effect.applyMultiple
                | None -> ()

                Scene.World) }
