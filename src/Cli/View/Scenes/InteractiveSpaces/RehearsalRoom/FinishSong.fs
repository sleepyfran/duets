module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.FinishSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.FinishSong

let rec finishSongScene space rooms =
    let state = State.Root.get ()
    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title = TextConstant FinishSongSelection
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = songOptions
                            Handler =
                                rehearsalRoomOptionalChoiceHandler
                                    space
                                    rooms
                                    (processSongSelection
                                        space
                                        rooms
                                        currentBand)
                            BackText = TextConstant CommonCancel } }
    }

and processSongSelection space rooms band selection =
    let state = State.Root.get ()

    let selectedSong =
        unfinishedSongFromSelection state band selection

    let (UnfinishedSong song, _, quality) = selectedSong

    seq {
        yield Effect <| finishSong band selectedSong

        yield
            FinishSongFinished(song.Name, quality)
            |> TextConstant
            |> Message

        yield Scene(Scene.RehearsalRoom(space, rooms))
    }
