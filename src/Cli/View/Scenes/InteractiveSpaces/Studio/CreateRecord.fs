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
let rec createRecordSubscene studio =
    let state = State.Root.get ()
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
                          <| trackListPrompt studio currentBand songOptions }
        else
            yield Message <| TextConstant StudioCreateNoSongs
            yield Scene World
    }

and trackListPrompt studio band songOptions name =
    seq {
        yield
            Prompt
                { Title = TextConstant StudioCreateTrackListPrompt
                  Content =
                      MultiChoicePrompt
                      <| { Choices = songOptions
                           Handler = processRecord studio band name } }
    }

and processRecord studio band name selectedSongs =
    let state = State.Root.get ()

    let albumResult =
        finishedSongsFromSelection state band selectedSongs
        |> Album.Unreleased.from name

    seq {
        match albumResult with
        | Error Album.NameTooShort ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooShort

            yield! createRecordSubscene studio
        | Error Album.NameTooLong ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooLong

            yield! createRecordSubscene studio
        | Ok album -> yield! confirmRecording studio band album
        | _ -> yield NoOp
    }

and confirmRecording studio band album =
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
                                              studio
                                              band
                                              album
                                  else
                                      yield Scene Scene.World
                              }) }
    }

and checkBankAndRecordAlbum studio band album =
    let state = State.Root.get ()

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

        yield!
            PromptToRelease.promptToReleaseAlbum
                (seq { yield Scene Scene.World })
                studio
                band
                album
    }
