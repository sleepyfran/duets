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

                showOptionalChoicePrompt
                    "Which album do you want to release?"
                    Generic.cancel
                    (fun (UnreleasedAlbum album) -> album.Name)
                    unreleasedAlbums
                |> Option.iter (Studio.promptToReleaseAlbum currentBand)

                Scene.World) }
