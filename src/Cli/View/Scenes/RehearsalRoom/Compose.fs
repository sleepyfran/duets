module Cli.View.Scenes.RehearsalRoom.Compose

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Cli.View.Scenes.RehearsalRoom.ComposeSong
open Cli.View.Scenes.RehearsalRoom.ImproveSong
open Cli.View.Scenes.RehearsalRoom.FinishSong
open Cli.View.Scenes.RehearsalRoom.DiscardSong
open Simulation.Bands.Queries
open Simulation.Songs.Queries

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

let rec compose state =
    let options =
        currentBand state
        |> fun band -> band.Id
        |> unfinishedSongsByBand state
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
                                    processSelection
                            BackText = TextConstant CommonCancel } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "compose_song" -> yield SubScene SubScene.RehearsalRoomComposeSong
        | "improve_song" -> yield SubScene SubScene.RehearsalRoomImproveSong
        | "finish_song" -> yield SubScene SubScene.RehearsalRoomFinishSong
        | "discard_song" -> yield SubScene SubScene.RehearsalRoomDiscardSong
        | _ -> yield NoOp
    }
