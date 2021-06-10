module Cli.View.Scenes.Studio.ContinueRecord

open Cli.View.Scenes.Studio.Common
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
let rec continueRecordSubscene state studio =
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
                            Handler = handleAlbum state studio currentBand } }
    }

and handleAlbum state studio band choice =
    let album =
        unreleasedAlbumFromSelection state band choice

    actionPrompt state studio band album

and actionPrompt state studio band album =
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
                                    (Scene <| Studio studio)
                                    (handleAction state studio band album)
                            BackText = TextConstant CommonBack } }
    }

and handleAction state studio band album choice =
    seq {
        match choice.Id with
        | "edit_name" -> yield! editName state studio band album
        | "release" ->
            yield!
                promptToReleaseAlbum
                    (seq { yield! actionPrompt state studio band album })
                    state
                    studio
                    band
                    album
        | _ -> yield NoOp
    }

and editName state studio band album =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioCreateRecordName
                  Content =
                      TextPrompt
                      <| handleNameChange state studio band album }
    }

and handleNameChange state studio band album name =
    renameAlbum band album name
    |> fun result ->
        match result with
        | Error Album.NameTooShort ->
            seq {
                yield
                    Message
                    <| TextConstant StudioCreateErrorNameTooShort

                yield! editName state studio band album
            }
        | Error Album.NameTooLong ->
            seq {
                yield
                    Message
                    <| TextConstant StudioCreateErrorNameTooLong

                yield! editName state studio band album
            }
        | Ok (album, effect) ->
            seq {
                yield!
                    Simulation.Galactus.runOne state effect
                    |> Seq.map Effect

                yield! actionPrompt state studio band album
            }
        | _ -> seq { yield NoOp }
