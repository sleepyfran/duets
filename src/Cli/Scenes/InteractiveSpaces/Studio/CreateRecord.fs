module Cli.Scenes.Studio.CreateRecord

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Queries
open Simulation.Bank.Operations
open Simulation.Studio.RecordAlbum

let rec createRecordSubscene studio =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let finishedSongs = Repertoire.allFinishedSongsByBand state currentBand.Id

    if List.isEmpty finishedSongs then
        StudioText StudioCreateNoSongs
        |> I18n.translate
        |> showMessage

        Scene.World
    else
        promptForName studio

and promptForName studio =
    showTextPrompt (
        StudioText StudioCreateRecordName
        |> I18n.translate
    )
    |> Album.validateName
    |> Result.switch
        (promptForTrackList studio)
        (showAlbumNameError
         >> fun _ -> promptForName studio)

and promptForTrackList studio name =
    let state = State.get ()
    let band = Bands.currentBand state

    let finishedSongs =
        Songs.finishedByBand state band.Id
        |> List.ofMapValues

    showMultiChoicePrompt
        (StudioText StudioCreateTrackListPrompt
         |> I18n.translate)
        (fun ((FinishedSong fs), currentQuality) ->
            CommonSongWithDetails(fs.Name, currentQuality, fs.Length)
            |> CommonText
            |> I18n.translate)
        finishedSongs
    |> promptForConfirmation studio band name

and promptForConfirmation studio band name selectedSongs =
    let album = Album.Unreleased.from name selectedSongs
    let (UnreleasedAlbum unreleasedAlbum) = album

    let confirmed =
        showConfirmationPrompt (
            StudioConfirmRecordingPrompt(
                unreleasedAlbum.Name,
                unreleasedAlbum.Type
            )
            |> StudioText
            |> I18n.translate
        )

    if confirmed then
        checkBankAndRecordAlbum studio band album
    else
        Scene.World

and checkBankAndRecordAlbum studio band album =
    let state = State.get ()

    match recordAlbum state studio band album with
    | Error (NotEnoughFunds studioBill) ->
        StudioCreateErrorNotEnoughMoney(studioBill)
        |> StudioText
        |> I18n.translate
        |> showMessage

        Scene.World
    | Ok (album, effects) -> recordWithProgressBar studio band album effects

and recordWithProgressBar _ band album effects =
    showProgressBar
        [ I18n.translate (StudioText StudioCreateProgressEatingSnacks)
          I18n.translate (StudioText StudioCreateProgressRecordingWeirdSounds)
          I18n.translate (StudioText StudioCreateProgressMovingKnobs) ]
        3<second>
        true

    List.iter Effect.apply effects

    PromptToRelease.promptToReleaseAlbum band album
