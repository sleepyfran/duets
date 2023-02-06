namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation
open Simulation.Bank.Operations
open Simulation.Studio.RecordAlbum

[<RequireQualifiedAccess>]
module CreateAlbumCommand =
    let rec private promptForName studio finishedSongs =
        showTextPrompt Studio.createRecordName
        |> Album.validateName
        |> Result.switch
            (promptForTrackList studio finishedSongs)
            (Studio.showAlbumNameError
             >> fun _ -> promptForName studio finishedSongs)

    and private promptForTrackList studio finishedSongs name =
        showOptionalChoicePrompt
            Studio.createTrackListPrompt
            Generic.cancel
            (fun (FinishedSong fs, currentQuality) ->
                Generic.songWithDetails fs.Name currentQuality fs.Length)
            finishedSongs
        |> Option.iter (promptForConfirmation studio finishedSongs name)

    and private promptForConfirmation studio finishedSongs name selectedSong =
        let FinishedSong fs, _ = selectedSong

        let confirmed =
            Studio.confirmRecordingPrompt fs.Name |> showConfirmationPrompt

        if confirmed then
            checkBankAndRecordAlbum studio name selectedSong
        else
            promptForTrackList studio finishedSongs name

    and private checkBankAndRecordAlbum studio albumName selectedSong =
        let state = State.get ()

        let band = Queries.Bands.currentBand state
        let result = startAlbum state studio band albumName selectedSong

        match result with
        | Ok effects -> recordWithProgressBar albumName effects
        | Error (NotEnoughFunds studioBill) ->
            Studio.createErrorNotEnoughMoney studioBill |> showMessage

    and private recordWithProgressBar albumName effects =
        showProgressBarAsync
            [ Studio.createProgressEatingSnacks
              Studio.createProgressRecordingWeirdSounds
              Studio.createProgressMovingKnobs ]
            3<second>

        List.iter Cli.Effect.apply effects
        Studio.createAlbumRecorded albumName |> showMessage

    /// Command to create a new album and potentially release.
    let create studio finishedSongs =
        { Name = "create album"
          Description =
            "Allows you to create a new album and record a song for it"
          Handler =
            (fun _ ->
                if List.isEmpty finishedSongs then
                    Studio.createNoSongs |> showMessage
                else
                    promptForName studio finishedSongs

                Scene.World) }
