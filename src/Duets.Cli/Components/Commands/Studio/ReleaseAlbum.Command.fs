namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

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
