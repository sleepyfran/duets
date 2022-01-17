module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.DiscardSong

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.DiscardSong

let rec discardSongSubScene () =
    let state = State.get ()

    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText DiscardSongSelection)
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

    let unfinishedSong =
        unfinishedSongFromSelection state band selection

    seq {

        yield Effect <| discardSong band unfinishedSong

        let (UnfinishedSong discardedSong, _, _) = unfinishedSong

        yield
            DiscardSongDiscarded discardedSong.Name
            |> RehearsalSpaceText
            |> I18n.translate
            |> Message

        yield Scene Scene.World
    }
