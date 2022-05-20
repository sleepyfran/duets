namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation

[<RequireQualifiedAccess>]
module ReleaseAlbumCommand =
    /// Command to release an unreleased album.
    let create unreleasedAlbums =
        { Name = "release album"
          Description =
            I18n.translate (CommandText CommandReleaseAlbumDescription)
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand =
                    Queries.Bands.currentBand state

                showChoicePrompt
                    (StudioText StudioContinueRecordPrompt
                     |> I18n.translate)
                    (fun (UnreleasedAlbum album) -> I18n.constant album.Name)
                    unreleasedAlbums
                |> Studio.promptToReleaseAlbum currentBand

                Scene.World) }
