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
        showTextPrompt (Studio.createRecordName)
        |> Album.validateName
        |> Result.switch
            (promptForTrackList studio finishedSongs)
            (Studio.showAlbumNameError
             >> fun _ -> promptForName studio finishedSongs)

    and private promptForTrackList studio finishedSongs name =
        let state = State.get ()
        let band = Queries.Bands.currentBand state

        showMultiChoicePrompt
            Studio.createTrackListPrompt
            (fun ((FinishedSong fs), currentQuality) ->
                Generic.songWithDetails fs.Name currentQuality fs.Length)
            finishedSongs
        |> promptForConfirmation studio band name

    and private promptForConfirmation studio band name selectedSongs =
        let album =
            Album.Unreleased.from name selectedSongs

        let (UnreleasedAlbum unreleasedAlbum) =
            album

        let confirmed =
            showConfirmationPrompt (
                Studio.confirmRecordingPrompt
                    unreleasedAlbum.Name
                    unreleasedAlbum.Type
            )

        if confirmed then
            checkBankAndRecordAlbum studio band album
        else
            ()

    and private checkBankAndRecordAlbum studio band album =
        let state = State.get ()

        match recordAlbum state studio band album with
        | Error (NotEnoughFunds studioBill) ->
            Studio.createErrorNotEnoughMoney (studioBill)
            |> showMessage
        | Ok (album, effects) -> recordWithProgressBar studio band album effects

    and private recordWithProgressBar _ band album effects =
        showProgressBarAsync
            [ Studio.createProgressEatingSnacks
              Studio.createProgressRecordingWeirdSounds
              Studio.createProgressMovingKnobs ]
            3<second>

        List.iter Cli.Effect.apply effects

        Studio.promptToReleaseAlbum band album

    /// Command to create a new album and potentially release.
    let create studio finishedSongs =
        { Name = "create album"
          Description = Command.createAlbumDescription
          Handler =
            (fun _ ->
                if List.isEmpty finishedSongs then
                    Studio.createNoSongs |> showMessage
                else
                    promptForName studio finishedSongs

                Scene.World) }
