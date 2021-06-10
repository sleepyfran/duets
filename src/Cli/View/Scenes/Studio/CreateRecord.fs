module Cli.View.Scenes.Studio.CreateRecord

open Cli.View.Scenes.Studio.Common
open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Queries
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
            yield SceneAfterKey Map
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
    let recordingResult =
        finishedSongsFromSelection state band selectedSongs
        |> recordAlbum state studio band name

    seq {
        match recordingResult with
        | Error NameTooShort ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooShort

            yield! createRecordSubscene state studio
        | Error NameTooLong ->
            yield
                Message
                <| TextConstant StudioCreateErrorNameTooLong

            yield! createRecordSubscene state studio
        | Error (NotEnoughMoney (bandBalance, studioBill)) ->
            yield
                StudioCreateErrorNotEnoughMoney(bandBalance, studioBill)
                |> TextConstant
                |> Message

            yield SceneAfterKey Map
        | Ok (album, effects) ->
            yield! recordWithProgressBar state studio band album effects
        | _ -> yield NoOp
    }

and recordWithProgressBar state studio band album effects =
    seq {
        yield
            ProgressBar
                { StepNames =
                      [ TextConstant StudioCreateProgressEatingSnacks
                        TextConstant StudioCreateProgressRecordingWeirdSounds
                        TextConstant StudioCreateProgressMovingKnobs ]
                  StepDuration = 3<second>
                  Async = true }

        yield!
            Simulation.Galactus.runMultiple state effects
            |> Seq.map Effect

        yield!
            promptToReleaseAlbum
                (seq { yield Scene <| Studio studio })
                state
                studio
                band
                album
    }
