module Cli.View.Scenes.RehearsalRoom.FinishSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Queries
open Simulation.Songs.Composition.FinishSong

let rec finishSongScene () =
  seq {
    let currentBand = currentBand ()

    let songOptions = unfinishedSongsSelectorOf currentBand

    yield
      Prompt
        { Title = TextConstant FinishSongSelection
          Content =
            ChoicePrompt
            <| OptionalChoiceHandler
                 { Choices = songOptions
                   Handler =
                     rehearsalRoomOptionalChoiceHandler
                     <| processSongSelection currentBand
                   BackText = TextConstant CommonCancel } }
  }

and processSongSelection band selection =
  seq {
    let selectedSong =
      unfinishedSongFromSelection band selection

    let (UnfinishedSong song, _, quality) = selectedSong

    selectedSong |> finishSong

    yield
      FinishSongFinished(song.Name, quality)
      |> TextConstant
      |> Message

    yield (Scene RehearsalRoom)
  }
