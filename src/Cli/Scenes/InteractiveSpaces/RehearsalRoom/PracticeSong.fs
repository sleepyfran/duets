module Cli.Scenes.InteractiveSpaces.RehearsalRoom.PracticeSong

open Agents
open Cli.Actions
open Cli.Common
open Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Practice

let finishedAndRecordedChoices state (band: Band) =
    Repertoire.allFinishedSongsByBand state band.Id
    |> List.map
        (fun (FinishedSong song, quality) ->
            let (SongId id) = song.Id

            { Id = id.ToString()
              Text =
                  PracticeSongItemDescription(song.Name, song.Practice)
                  |> RehearsalSpaceText
                  |> I18n.translate })

let rec practiceSongSubScene () =
    let state = State.get ()
    let band = Bands.currentBand state

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText ImproveSongSelection)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = finishedAndRecordedChoices state band
                            Handler =
                                worldOptionalChoiceHandler (
                                    processSongSelection band
                                )
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and processSongSelection band selection =
    let state = State.get ()

    let effects =
        Repertoire.finishedFromAllByBandAndSongId
            state
            band.Id
            (SongId(System.Guid.Parse selection.Id))
        |> Option.get
        |> practiceSong band

    seq {
        match effects with
        | SongImproved effect ->
            yield practiceProgress ()
            yield Effect effect
        | SongAlreadyImprovedToMax (FinishedSong song, _) ->
            yield
                PracticeSongAlreadyImprovedToMax song.Name
                |> RehearsalSpaceText
                |> I18n.translate
                |> Message

        yield Scene Scene.World
    }

and practiceProgress () =
    ProgressBar
        { StepNames =
              [ I18n.translate (
                  RehearsalSpaceText PracticeSongProgressLosingTime
                )
                I18n.translate (
                    RehearsalSpaceText PracticeSongProgressTryingSoloOnceMore
                )
                I18n.translate (RehearsalSpaceText PracticeSongProgressGivingUp) ]
          StepDuration = 2<second>
          Async = true }
