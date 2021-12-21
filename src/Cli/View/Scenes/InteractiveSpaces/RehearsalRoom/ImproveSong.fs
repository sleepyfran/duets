module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ImproveSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongScene state space rooms =
    seq {
        let currentBand = Bands.currentBand state

        let songOptions =
            unfinishedSongsSelectorOf state currentBand

        yield
            Prompt
                { Title = TextConstant ImproveSongSelection
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = songOptions
                            Handler =
                                (processOptionalSongSelection
                                    state
                                    space
                                    rooms
                                    currentBand)
                            BackText = TextConstant CommonCancel } }
    }

and processOptionalSongSelection state space rooms band selection =
    seq {
        match selection with
        | Choice choice ->
            yield! processSongSelection state space rooms band choice
        | Back -> yield Scene(Scene.RehearsalRoom(space, rooms))
    }

and processSongSelection state space rooms band selection =
    seq {
        let status =
            unfinishedSongFromSelection state band selection
            |> improveSong band

        match status with
        | (CanBeImproved, effects) ->
            yield showImprovingProgress ()
            yield! List.map Effect effects
        | (ReachedMaxQualityInLastImprovement, effects) ->
            yield showImprovingProgress ()
            yield! List.map Effect effects
            yield bandFinishedSong
        | (ReachedMaxQualityAlready, _) -> yield bandFinishedSong

        yield Scene(Scene.RehearsalRoom(space, rooms))
    }

and showImprovingProgress () =
    ProgressBar
        { StepNames =
              [ TextConstant ImproveSongProgressAddingSomeMelodies
                TextConstant ImproveSongProgressPlayingFoosball
                TextConstant ImproveSongProgressModifyingChordsFromAnotherSong ]
          StepDuration = 2<second>
          Async = true }

and bandFinishedSong =
    ImproveSongReachedMaxQuality
    |> TextConstant
    |> Message
