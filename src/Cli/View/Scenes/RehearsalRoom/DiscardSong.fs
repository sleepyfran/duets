module Cli.View.Scenes.RehearsalRoom.DiscardSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities.Types
open Simulation.Bands.Queries
open Simulation.Songs.Composition.DiscardSong

let rec discardSongScene () =
  seq {
    let currentBand = currentBand ()

    let songOptions = unfinishedSongsSelectorOf currentBand

    yield
      Prompt
        { Title = TextConstant DiscardSongSelection
          Content = ChoicePrompt(songOptions, processSongSelection currentBand) }
  }

and processSongSelection band selection =
  seq {
    let (UnfinishedSong discardedSong, _, _) =
      unfinishedSongFromSelection band selection
      |> discardSong

    yield
      DiscardSongDiscarded discardedSong.Name
      |> TextConstant
      |> Message

    yield (Scene RehearsalRoom)
  }
