module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.FinishSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.FinishSong

let rec finishSongScene state space rooms =
    seq {
        let currentBand = Bands.currentBand state

        let songOptions =
            unfinishedSongsSelectorOf state currentBand

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
                                        state
                                        space
                                        rooms
                                        currentBand)
                            BackText = TextConstant CommonCancel } }
    }

and processSongSelection state space rooms band selection =
    seq {
        let selectedSong =
            unfinishedSongFromSelection state band selection

        let (UnfinishedSong song, _, quality) = selectedSong

        yield Effect <| finishSong band selectedSong

        yield
            FinishSongFinished(song.Name, quality)
            |> TextConstant
            |> Message

        yield Scene(Scene.RehearsalRoom(space, rooms))
    }
