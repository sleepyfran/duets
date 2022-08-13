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
          Description = Command.releaseAlbumDescription
          Handler =
            (fun _ ->
                let state = State.get ()

                let currentBand = Queries.Bands.currentBand state

                showChoicePrompt
                    Studio.continueRecordPrompt
                    (fun (UnreleasedAlbum album) -> album.Name)
                    unreleasedAlbums
                |> Studio.promptToReleaseAlbum currentBand

                Scene.World) }
