module Cli.View.Scenes.RehearsalRoom.ImproveSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Bands.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongScene () =
  seq {
    let currentBand = currentBand ()
    let songOptions = unfinishedSongsSelectorOf currentBand

    yield
      Prompt
        { Title = TextConstant ImproveSongSelection
          Content =
            ChoicePrompt
            <| OptionalChoiceHandler
                 { Choices = songOptions
                   Handler = processOptionalSongSelection currentBand
                   BackText = TextConstant CommonCancel } }
  }

and processOptionalSongSelection band selection =
  seq {
    match selection with
    | Choice choice -> yield! processSongSelection band choice
    | Back -> yield Scene RehearsalRoom
  }

and processSongSelection band selection =
  seq {
    let songStatus =
      unfinishedSongFromSelection band selection
      |> improveSong

    match songStatus with
    | CanBeImproved quality ->
        yield showImprovingProgress ()

        yield
          ImproveSongCanBeFurtherImproved quality
          |> TextConstant
          |> Message
    | ReachedMaxQualityInLastImprovement quality ->
        yield showImprovingProgress ()
        yield bandFinishedSong quality
    | ReachedMaxQualityAlready quality -> yield bandFinishedSong quality

    yield (Scene RehearsalRoom)
  }

and showImprovingProgress () =
  ProgressBar
    { StepNames =
        [ TextConstant ImproveSongProgressAddingSomeMelodies
          TextConstant ImproveSongProgressPlayingFoosball
          TextConstant ImproveSongProgressModifyingChordsFromAnotherSong ]
      StepDuration = 2<second>
      Async = true }

and bandFinishedSong quality =
  ImproveSongReachedMaxQuality quality
  |> TextConstant
  |> Message
