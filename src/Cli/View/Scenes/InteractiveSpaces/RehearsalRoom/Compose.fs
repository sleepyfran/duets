module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Compose

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Simulation.Queries

let createOptions hasUnfinishedSongs =
    seq {
        yield
            { Id = "compose_song"
              Text = TextConstant ComposeSong }

        if hasUnfinishedSongs then
            yield
                { Id = "improve_song"
                  Text = TextConstant ImproveSong }

            yield
                { Id = "finish_song"
                  Text = TextConstant FinishSong }

            yield
                { Id = "discard_song"
                  Text = TextConstant DiscardSong }
    }
    |> List.ofSeq

let rec composeSubScene space rooms =
    let state = State.Root.get ()

    let options =
        Bands.currentBand state
        |> fun band -> band.Id
        |> Songs.unfinishedByBand state
        |> Map.count
        |> fun count -> count > 0
        |> createOptions

    seq {
        yield
            Prompt
                { Title = TextConstant ComposePrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = options
                            Handler =
                                rehearsalRoomOptionalChoiceHandler
                                    space
                                    rooms
                                    (processSelection space rooms)
                            BackText = TextConstant CommonCancel } }
    }

and processSelection space rooms choice =
    seq {
        match choice.Id with
        | "compose_song" -> yield! ComposeSong.composeSongSubScene space rooms
        | "improve_song" -> yield! ImproveSong.improveSongSubScene space rooms
        | "finish_song" -> yield! FinishSong.finishSongSubScene space rooms
        | "discard_song" -> yield! DiscardSong.discardSongSubScene space rooms
        | _ -> yield NoOp
    }
