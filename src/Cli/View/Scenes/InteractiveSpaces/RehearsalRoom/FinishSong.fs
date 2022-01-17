module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.FinishSong

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.FinishSong

let rec finishSongSubScene () =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText FinishSongSelection)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = songOptions
                            Handler =
                                worldOptionalChoiceHandler (
                                    processSongSelection currentBand
                                )
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and processSongSelection band selection =
    let state = State.get ()

    let selectedSong =
        unfinishedSongFromSelection state band selection

    let (UnfinishedSong song, _, quality) = selectedSong

    seq {
        yield Effect <| finishSong band selectedSong

        yield
            FinishSongFinished(song.Name, quality)
            |> RehearsalSpaceText
            |> I18n.translate
            |> Message

        yield Scene Scene.World
    }
