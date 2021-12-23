module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ImproveSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongScene space rooms =
    let state = State.Root.get ()

    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title = TextConstant ImproveSongSelection
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = songOptions
                            Handler =
                                (processOptionalSongSelection
                                    space
                                    rooms
                                    currentBand)
                            BackText = TextConstant CommonCancel } }
    }

and processOptionalSongSelection space rooms band selection =
    seq {
        match selection with
        | Choice choice -> yield! processSongSelection space rooms band choice
        | Back -> yield Scene(Scene.RehearsalRoom(space, rooms))
    }

and processSongSelection space rooms band selection =
    let state = State.Root.get ()

    let status =
        unfinishedSongFromSelection state band selection
        |> improveSong band

    seq {
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
