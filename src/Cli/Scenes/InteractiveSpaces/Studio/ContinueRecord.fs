module Cli.Scenes.Studio.ContinueRecord

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation.Studio.RenameAlbum
open Simulation.Queries

type private ContinueRecordMenuOption =
    | EditName
    | Release

let private textFromOption opt =
    match opt with
    | EditName -> StudioText StudioContinueRecordActionPromptEditName
    | Release -> StudioText StudioContinueRecordActionPromptRelease
    |> I18n.translate

/// Creates a subscene that allows to edit the name of a previously recorded
/// but unreleased album and also to release it.
let rec continueRecordSubscene studio = promptForAlbum studio

and private promptForAlbum studio =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let unreleasedAlbums =
        Albums.unreleasedByBand state currentBand.Id
        |> List.ofMapValues

    showChoicePrompt
        (StudioText StudioContinueRecordPrompt
         |> I18n.translate)
        (fun (UnreleasedAlbum album) -> I18n.constant album.Name)
        unreleasedAlbums
    |> promptForAlbumAction studio currentBand

and private promptForAlbumAction studio band album =
    let action =
        showChoicePrompt
            (StudioText StudioContinueRecordActionPrompt
             |> I18n.translate)
            textFromOption
            [ EditName; Release ]

    match action with
    | EditName -> promptForAlbumName studio band album
    | Release -> PromptToRelease.promptToReleaseAlbum band album

and private promptForAlbumName studio band album =
    let name =
        showTextPrompt (
            StudioText StudioCreateRecordName
            |> I18n.translate
        )

    Album.validateName name
    |> Result.switch
        (fun name ->
            renameAlbum band album name |> Effect.apply
            Scene.World)
        (showAlbumNameError
         >> fun _ -> promptForAlbumName studio band album)
