module Cli.View.Scenes.Studio.CreateRecord

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Bank.Operations
open Simulation.Studio.RecordAlbum

/// Creates the studio record subscene which allows bands to create a new
/// record.
let rec createRecordSubscene state studio =
    let currentBand = Bands.currentBand state

    let songOptions =
        finishedSongsSelectorOf state currentBand

    seq {
        if songOptions.Length > 0 then
            yield
                Prompt
                    { Title = TextConstant StudioCreateRecordName
                      Content =
                          TextPrompt
                          <| trackListPrompt
                              state
                              studio
                              currentBand
                              songOptions }
        else
            yield Message <| TextConstant StudioCreateNoSongs
            yield Scene World
    }

and trackListPrompt state studio band songOptions name =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioCreateTrackListPrompt
                  Content =
                      MultiChoicePrompt
                      <| { Choices = songOptions
                           Handler = processRecord state studio band name } }
    }

and processRecord state studio band name selectedSongs =
    let albumResult =
        finishedSongsFromSelection state band selectedSongs
        |> Album.Unreleased.from name

    seq {
        match albumResult with
        | Error Album.NameTooShort ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooShort

            yield! createRecordSubscene state studio
        | Error Album.NameTooLong ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooLong

            yield! createRecordSubscene state studio
        | Ok album -> yield! confirmRecording state studio band album
        | _ -> yield NoOp
    }

and confirmRecording state studio band album =
    let (UnreleasedAlbum albumToRecord) = album

    seq {
        yield
            Prompt
                { Title =
                      StudioConfirmRecordingPrompt(
                          albumToRecord.Name,
                          albumToRecord.Type
                      )
                      |> TextConstant
                  Content =
                      ConfirmationPrompt
                          (fun confirmed ->
                              seq {
                                  if confirmed then
                                      yield!
                                          checkBankAndRecordAlbum
                                              state
                                              studio
                                              band
                                              album
                                  else
                                      yield Scene(Scene.Studio studio)
                              }) }
    }

and checkBankAndRecordAlbum state studio band album =
    seq {
        match recordAlbum state studio band album with
        | Error (NotEnoughFunds studioBill) ->
            yield
                StudioCreateErrorNotEnoughMoney(studioBill)
                |> TextConstant
                |> Message

            yield Scene World
        | Ok (album, effects) ->
            yield! recordWithProgressBar studio band album effects
    }

and recordWithProgressBar studio band album effects =
    seq {
        yield
            ProgressBar
                { StepNames =
                      [ TextConstant StudioCreateProgressEatingSnacks
                        TextConstant StudioCreateProgressRecordingWeirdSounds
                        TextConstant StudioCreateProgressMovingKnobs ]
                  StepDuration = 3<second>
                  Async = true }

        yield! Seq.map Effect effects

        yield
            StudioPromptToRelease(
                (seq { yield Scene <| Scene.Studio studio }),
                studio,
                band,
                album
            )
            |> SubScene
    }
