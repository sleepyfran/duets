namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
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
                finishedSong,
                Concert.Ongoing.hasPlayedSong ongoingConcert finishedSong)
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

    /// Command which simulates playing a song in a concert.
    let createPlaySong ongoingConcert =
        { Name = "play song"
          Description = Command.playDescription
          Handler =
            (fun _ ->
                let selectedSong = promptForSong ongoingConcert

                match selectedSong with
                | Some(finishedSong, _) ->
                    let energy = promptForEnergy ()

                    Effect.applyAction (
                        ConcertPerformAction
                            {| Action = PlaySong(finishedSong, energy)
                               Concert = ongoingConcert |}
                    )
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
                | Some(finishedSong, _) ->
                    let energy = promptForEnergy ()

                    Effect.applyAction (
                        ConcertPerformAction
                            {| Action = DedicateSong(finishedSong, energy)
                               Concert = ongoingConcert |}
                    )
                | None -> ()

                Scene.World) }
