module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Compose

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Simulation.Queries

let createOptions hasUnfinishedSongs =
    seq {
        yield
            { Id = "compose_song"
              Text = I18n.translate (RehearsalSpaceText ComposeSong) }

        if hasUnfinishedSongs then
            yield
                { Id = "improve_song"
                  Text = I18n.translate (RehearsalSpaceText ImproveSong) }

            yield
                { Id = "finish_song"
                  Text = I18n.translate (RehearsalSpaceText FinishSong) }

            yield
                { Id = "discard_song"
                  Text = I18n.translate (RehearsalSpaceText DiscardSong) }
    }
    |> List.ofSeq

let rec composeSubScene () =
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
                { Title = I18n.translate (RehearsalSpaceText ComposePrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = options
                            Handler =
                                worldOptionalChoiceHandler processSelection
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "compose_song" -> yield! ComposeSong.composeSongSubScene ()
        | "improve_song" -> yield! ImproveSong.improveSongSubScene ()
        | "finish_song" -> yield! FinishSong.finishSongSubScene ()
        | "discard_song" -> yield! DiscardSong.discardSongSubScene ()
        | _ -> yield NoOp
    }
