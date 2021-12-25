module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.DiscardSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.DiscardSong

let rec discardSongSubScene () =
    let state = State.Root.get ()

    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title = TextConstant DiscardSongSelection
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = songOptions
                            Handler =
                                worldOptionalChoiceHandler (
                                    processSongSelection currentBand
                                )
                            BackText = TextConstant CommonCancel } }
    }

and processSongSelection band selection =
    let state = State.Root.get ()

    let unfinishedSong =
        unfinishedSongFromSelection state band selection

    seq {

        yield Effect <| discardSong band unfinishedSong

        let (UnfinishedSong discardedSong, _, _) = unfinishedSong

        yield
            DiscardSongDiscarded discardedSong.Name
            |> TextConstant
            |> Message

        yield Scene Scene.World
    }
