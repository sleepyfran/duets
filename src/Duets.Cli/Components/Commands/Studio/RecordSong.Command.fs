namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Studio.RecordAlbum

[<RequireQualifiedAccess>]
module RecordSongCommand =
    let rec private promptForSong studio unreleasedAlbums finishedSongs =
        showOptionalChoicePrompt
            $"""Select which {Styles.highlight "song"} will be added to the album"""
            Generic.cancel
            (fun (FinishedSong fs, currentQuality) ->
                Generic.songWithDetails fs.Name currentQuality fs.Length)
            finishedSongs
        |> Option.iter (promptForAlbum studio unreleasedAlbums finishedSongs)

    and private promptForAlbum studio unreleasedAlbums finishedSongs song =
        let FinishedSong fs, _ = song

        showOptionalChoicePrompt
            $"Which album do you want to {fs.Name} to?"
            Generic.cancel
            (fun (UnreleasedAlbum album) -> album.Name)
            unreleasedAlbums
        |> Option.iter (
            promptForConfirmation studio unreleasedAlbums finishedSongs song
        )

    and private promptForConfirmation
        studio
        unreleasedAlbums
        finishedSongs
        song
        unreleasedAlbum
        =
        let album = Album.fromUnreleased unreleasedAlbum
        let FinishedSong fs, _ = song

        let confirmed =
            $"Are you sure you want to add {fs.Name} to the album {album.Name}"
            |> showConfirmationPrompt

        if confirmed then
            checkBankAndRecordSong studio unreleasedAlbum song
        else
            promptForSong studio unreleasedAlbums finishedSongs

    and private checkBankAndRecordSong studio album song =
        let state = State.get ()

        let band = Queries.Bands.currentBand state
        let result = recordSongForAlbum state studio band album song

        match result with
        | Ok effects -> recordWithProgressBar album song effects
        | Error (NotEnoughFunds studioBill) ->
            Studio.createErrorNotEnoughMoney studioBill |> showMessage

    and private recordWithProgressBar
        (UnreleasedAlbum album)
        (FinishedSong song, _)
        effects
        =
        showProgressBarAsync
            [ Studio.createProgressEatingSnacks
              Studio.createProgressRecordingWeirdSounds
              Studio.createProgressMovingKnobs ]
            3<second>

        $"Added {song.Name} to {album.Name}. It is now a {Generic.albumType album.Type}"
        |> Styles.success
        |> showMessage
        
        List.iter Duets.Cli.Effect.apply effects

    /// Command to record a new song for a previously started album.
    let create studio unreleasedAlbums finishedSongs =
        { Name = "record song"
          Description = "Allows you to record a song and add it to a previously created album that hasn't been released yet"
          Handler =
            (fun _ ->
                if List.isEmpty finishedSongs then
                    Studio.createNoSongs |> showMessage
                else
                    promptForSong studio unreleasedAlbums finishedSongs

                Scene.World) }
