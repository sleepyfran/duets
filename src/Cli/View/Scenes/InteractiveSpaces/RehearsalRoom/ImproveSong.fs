module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.ImproveSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongSubScene () =
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
                                worldOptionalChoiceHandler (
                                    processSongSelection currentBand
                                )
                            BackText = TextConstant CommonCancel } }
    }

and processSongSelection band selection =
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

        yield Scene Scene.World
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
