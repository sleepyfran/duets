module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ChooseAction

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Simulation.Queries

let private createOptions hasUnfinishedSongs hasFinishedSongs =
    seq {
        { Id = "compose_song"
          Text = I18n.translate (RehearsalSpaceText ComposeSong) }

        if hasUnfinishedSongs then
            { Id = "improve_song"
              Text = I18n.translate (RehearsalSpaceText ImproveSong) }

            { Id = "finish_song"
              Text = I18n.translate (RehearsalSpaceText FinishSong) }

            { Id = "discard_song"
              Text = I18n.translate (RehearsalSpaceText DiscardSong) }

        if hasFinishedSongs then
            { Id = "practice_song"
              Text = I18n.translate (RehearsalSpaceText PracticeSong) }
    }
    |> List.ofSeq

let rec createMenu () =
    let state = State.get ()
    let band = Bands.currentBand state

    let hasUnfinishedSongs =
        Songs.unfinishedByBand state band.Id
        |> Map.isEmpty
        |> not

    let hasFinishedSongs =
        Repertoire.allFinishedSongsByBand state band.Id
        |> List.isEmpty
        |> not

    seq {
        yield
            Prompt
                { Title = I18n.translate (RehearsalSpaceText ComposePrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices =
                                createOptions
                                    hasUnfinishedSongs
                                    hasFinishedSongs
                            Handler =
                                worldOptionalChoiceHandler processSelection
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and private processSelection choice =
    seq {
        match choice.Id with
        | "compose_song" -> yield! ComposeSong.composeSongSubScene ()
        | "improve_song" -> yield! ImproveSong.improveSongSubScene ()
        | "finish_song" -> yield! FinishSong.finishSongSubScene ()
        | "practice_song" -> yield! PracticeSong.practiceSongSubScene ()
        | "discard_song" -> yield! DiscardSong.discardSongSubScene ()
        | _ -> yield NoOp
    }
