module Cli.Scenes.InteractiveSpaces.RehearsalRoom.ImproveSong

open Agents
open Cli.Actions
open Cli.Common
open Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongSubScene () =
    let state = State.get ()

    let currentBand = Bands.currentBand state

    let songOptions =
        unfinishedSongsSelectorOf state currentBand

    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText ImproveSongSelection)
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
              [ I18n.translate (
                  RehearsalSpaceText ImproveSongProgressAddingSomeMelodies
                )
                I18n.translate (
                    RehearsalSpaceText ImproveSongProgressPlayingFoosball
                )
                I18n.translate (
                    RehearsalSpaceText
                        ImproveSongProgressModifyingChordsFromAnotherSong
                ) ]
          StepDuration = 2<second>
          Async = true }

and bandFinishedSong =
    I18n.translate (RehearsalSpaceText ImproveSongReachedMaxQuality)
    |> Message
