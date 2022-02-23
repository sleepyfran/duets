module Cli.Scenes.InteractiveSpaces.RehearsalRoom.ImproveSong

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation.Queries
open Simulation.Songs.Composition.ImproveSong

let rec improveSongSubScene () = promptForSong ()

and private promptForSong () =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let songs =
        Songs.unfinishedByBand state currentBand.Id
        |> List.ofMapValues

    let selectedSong =
        showOptionalChoicePrompt
            (RehearsalSpaceText DiscardSongSelection
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (UnfinishedSong us, _, currentQuality) ->
                CommonSongWithDetails(us.Name, currentQuality, us.Length)
                |> CommonText
                |> I18n.translate)
            songs

    match selectedSong with
    | Some song -> showImproveSong currentBand song
    | None -> ()

    Scene.World

and private showImproveSong band song =
    let improveStatus = improveSong band song

    match improveStatus with
    | CanBeImproved, effects ->
        showImprovingProgress ()
        List.iter Cli.Effect.apply effects
    | ReachedMaxQualityInLastImprovement, effects ->
        showImprovingProgress ()
        List.iter Cli.Effect.apply effects

        RehearsalSpaceText ImproveSongReachedMaxQuality
        |> I18n.translate
        |> showMessage
    | ReachedMaxQualityAlready, _ ->
        RehearsalSpaceText ImproveSongReachedMaxQuality
        |> I18n.translate
        |> showMessage

and showImprovingProgress () =
    showProgressBar
        [ I18n.translate (
              RehearsalSpaceText ImproveSongProgressAddingSomeMelodies
          )
          I18n.translate (RehearsalSpaceText ImproveSongProgressPlayingFoosball)
          I18n.translate (
              RehearsalSpaceText
                  ImproveSongProgressModifyingChordsFromAnotherSong
          ) ]
        2<second>
        true
