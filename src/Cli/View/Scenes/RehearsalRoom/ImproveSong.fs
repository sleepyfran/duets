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
          Content = ChoicePrompt(songOptions, processSongSelection currentBand) }
  }

and processSongSelection band selection =
  seq {
    let songStatus =
      unfinishedSongFromSelection band selection
      |> improveSong

    match songStatus with
    | CanBeImproved quality ->
        yield
          ProgressBar
            { StepNames =
                [ TextConstant ImproveSongProgressAddingSomeMelodies
                  TextConstant ImproveSongProgressPlayingFoosball
                  TextConstant ImproveSongProgressModifyingChordsFromAnotherSong ]
              StepDuration = 2<second>
              Async = true }

        yield
          ImproveSongCanBeFurtherImproved quality
          |> TextConstant
          |> Message
    | ReachedMaxQuality quality ->
        yield
          ImproveSongReachedMaxQuality quality
          |> TextConstant
          |> Message

    yield (Scene RehearsalRoom)
  }