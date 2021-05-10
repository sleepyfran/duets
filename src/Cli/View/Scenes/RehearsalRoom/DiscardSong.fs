module Cli.View.Scenes.RehearsalRoom.DiscardSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Queries
open Simulation.Songs.Composition.DiscardSong

let rec discardSongScene state =
  seq {
    let currentBand = currentBand state

    let songOptions =
      unfinishedSongsSelectorOf state currentBand

    yield
      Prompt
        { Title = TextConstant DiscardSongSelection
          Content =
            ChoicePrompt
            <| OptionalChoiceHandler
                 { Choices = songOptions
                   Handler =
                     rehearsalRoomOptionalChoiceHandler
                     <| processSongSelection state currentBand
                   BackText = TextConstant CommonCancel } }
  }

and processSongSelection state band selection =
  seq {
    let unfinishedSong =
      unfinishedSongFromSelection state band selection

    yield Effect <| discardSong band unfinishedSong

    let (UnfinishedSong discardedSong, _, _) = unfinishedSong

    yield
      DiscardSongDiscarded discardedSong.Name
      |> TextConstant
      |> Message

    yield SceneAfterKey RehearsalRoom
  }
