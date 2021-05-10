module Cli.View.Scenes.RehearsalRoom.ImproveSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Bands.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongScene state =
    seq {
        let currentBand = currentBand state

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
                                (processOptionalSongSelection state currentBand)
                            BackText = TextConstant CommonCancel } }
    }

and processOptionalSongSelection state band selection =
    seq {
        match selection with
        | Choice choice -> yield! processSongSelection state band choice
        | Back -> yield Scene RehearsalRoom
    }

and processSongSelection state band selection =
    seq {
        let (songStatus, effects) =
            unfinishedSongFromSelection state band selection
            |> improveSong state band

        yield! effects |> Seq.map Effect

        match songStatus with
        | CanBeImproved quality ->
            yield showImprovingProgress ()

            yield
                ImproveSongCanBeFurtherImproved quality
                |> TextConstant
                |> Message
        | ReachedMaxQualityInLastImprovement quality ->
            yield showImprovingProgress ()
            yield bandFinishedSong quality
        | ReachedMaxQualityAlready quality -> yield bandFinishedSong quality

        yield SceneAfterKey RehearsalRoom
    }

and showImprovingProgress () =
    ProgressBar
        { StepNames =
              [ TextConstant ImproveSongProgressAddingSomeMelodies
                TextConstant ImproveSongProgressPlayingFoosball
                TextConstant ImproveSongProgressModifyingChordsFromAnotherSong ]
          StepDuration = 2<second>
          Async = true }

and bandFinishedSong quality =
    ImproveSongReachedMaxQuality quality
    |> TextConstant
    |> Message
