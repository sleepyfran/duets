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

            Rehearsal.improveSongReachedMaxQuality
            |> showMessage
        | ReachedMaxQualityAlready, _ ->
            Rehearsal.improveSongReachedMaxQuality
            |> showMessage

    and private showImprovingProgress () =
        showProgressBarAsync
            [ Rehearsal.improveSongProgressAddingSomeMelodies
              Rehearsal.improveSongProgressPlayingFoosball
              Rehearsal.improveSongProgressModifyingChordsFromAnotherSong ]
            2<second>

    /// Command to improve an unfinished song.
    let create unfinishedSongs =
        { Name = "improve song"
          Description = Command.improveSongDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.improveSongSelection
                        Generic.cancel
                        (fun (UnfinishedSong us, _, currentQuality) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song -> improveSong currentBand song |> showImproveSong
                | None -> ()

                Scene.World) }
