namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames
open Entities
open Simulation
open Simulation.Songs.Composition.ImproveSong

[<RequireQualifiedAccess>]
module ImproveSongCommand =
    let rec private showImproveSong status =
        match status with
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

    and private showImprovingProgress () =
        showProgressBarAsync
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
            2<second>

    /// Command to improve an unfinished song.
    let create unfinishedSongs =
        { Name = "improve song"
          Description =
            I18n.translate (CommandText CommandImproveSongDescription)
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        (RehearsalSpaceText ImproveSongSelection
                         |> I18n.translate)
                        (CommonText CommonCancel |> I18n.translate)
                        (fun (UnfinishedSong us, _, currentQuality) ->
                            CommonSongWithDetails(
                                us.Name,
                                currentQuality,
                                us.Length
                            )
                            |> CommonText
                            |> I18n.translate)
                        unfinishedSongs

                match selectedSong with
                | Some song -> improveSong currentBand song |> showImproveSong
                | None -> ()

                Scene.World) }
