namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Songs.Composition.ImproveSong

[<RequireQualifiedAccess>]
module ImproveSongCommand =
    let rec private showImproveSong status =
        match status with
        | CanBeImproved, effects ->
            showImprovingProgress ()
            List.iter Duets.Cli.Effect.apply effects
        | ReachedMaxQualityInLastImprovement, effects ->
            showImprovingProgress ()
            Rehearsal.improveSongReachedMaxQuality |> showMessage
            List.iter Duets.Cli.Effect.apply effects
        | ReachedMaxQualityAlready, _ ->
            Rehearsal.improveSongReachedMaxQuality |> showMessage

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

                let currentBand = Queries.Bands.currentBand state

                let selectedSong =
                    showOptionalChoicePrompt
                        Rehearsal.improveSongSelection
                        Generic.cancel
                        (fun (Unfinished(us: Song, _, currentQuality)) ->
                            Generic.songWithDetails
                                us.Name
                                currentQuality
                                us.Length)
                        unfinishedSongs

                match selectedSong with
                | Some song ->
                    improveSong (State.get ()) currentBand song
                    |> showImproveSong
                | None -> ()

                Scene.World) }
