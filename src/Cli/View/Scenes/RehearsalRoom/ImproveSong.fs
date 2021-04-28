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

    if songOptions.Length = 0 then
      yield Message <| TextConstant CommonNoUnfinishedSongs
      yield (Scene RehearsalRoom)
    else
      yield
        Prompt
          { Title = TextConstant ImproveSongSelection
            Content =
              ChoicePrompt(songOptions, processSongSelection currentBand) }
  }

and processSongSelection band selection =
  seq {
    let songStatus =
      unfinishedSongFromSelection band selection
      |> improveSong

    yield
      ProgressBar
        { StepNames =
            [ TextConstant ImproveSongProgressAddingSomeMelodies
              TextConstant ImproveSongProgressPlayingFoosball
              TextConstant ImproveSongProgressModifyingChordsFromAnotherSong ]
          StepDuration = 2<second>
          Async = true }

    match songStatus with
    | CanBeImproved quality ->
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
