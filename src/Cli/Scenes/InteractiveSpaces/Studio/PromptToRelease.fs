module Cli.Scenes.Studio.PromptToRelease

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation.Studio.ReleaseAlbum

/// Shows a prompt that asks the user if they want to release an album and
/// handles the release.
let rec promptToReleaseAlbum band unreleasedAlbum =
    let (UnreleasedAlbum album) = unreleasedAlbum
    let state = State.get ()

    let confirmed =
        showConfirmationPrompt (
            StudioCommonPromptReleaseAlbum album.Name
            |> StudioText
            |> I18n.translate
        )

    if confirmed then
        releaseAlbum state band unreleasedAlbum
        |> Effect.apply

    Scene.World
