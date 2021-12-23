module Cli.View.Scenes.Studio.ContinueRecord

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Studio.RenameAlbum
open Simulation.Queries

let continueRecordOptions =
    [ { Id = "edit_name"
        Text = TextConstant StudioContinueRecordActionPromptEditName }
      { Id = "release"
        Text = TextConstant StudioContinueRecordActionPromptRelease } ]

/// Creates a subscene that allows to edit the name of a previously recorded
/// but unreleased album and also to release it.
let rec continueRecordSubscene studio =
    let state = State.Root.get ()
    let currentBand = Bands.currentBand state

    let albumOptions =
        unreleasedAlbumsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title = TextConstant StudioContinueRecordPrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = albumOptions
                            Handler = handleAlbum studio currentBand } }
    }

and handleAlbum studio band choice =
    let state = State.Root.get ()

    let album =
        unreleasedAlbumFromSelection state band choice

    actionPrompt studio band album

and actionPrompt studio band album =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioContinueRecordActionPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = continueRecordOptions
                            Handler =
                                basicOptionalChoiceHandler
                                    (Scene <| Scene.Studio studio)
                                    (handleAction studio band album)
                            BackText = TextConstant CommonBack } }
    }

and handleAction studio band album choice =
    seq {
        match choice.Id with
        | "edit_name" -> yield! editName studio band album
        | "release" ->
            yield!
                PromptToRelease.promptToReleaseAlbum
                    (seq { yield! actionPrompt studio band album })
                    studio
                    band
                    album
        | _ -> yield NoOp
    }

and editName studio band album =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioCreateRecordName
                  Content = TextPrompt <| handleNameChange studio band album }
    }

and handleNameChange studio band album name =
    renameAlbum band album name
    |> fun result ->
        match result with
        | Error Album.NameTooShort ->
            seq {
                yield
                    Message
                    <| TextConstant StudioCreateErrorNameTooShort

                yield! editName studio band album
            }
        | Error Album.NameTooLong ->
            seq {
                yield
                    Message
                    <| TextConstant StudioCreateErrorNameTooLong

                yield! editName studio band album
            }
        | Ok (album, effect) ->
            seq {
                yield Effect effect
                yield! actionPrompt studio band album
            }
        | _ -> seq { yield NoOp }
