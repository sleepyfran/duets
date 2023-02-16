namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Studio.ReleaseAlbum

module Studio =
    /// Shows an error indicating what made the album name validation fail.
    let showAlbumNameError error =
        match error with
        | Album.NameTooShort -> Studio.createErrorNameTooShort
        | Album.NameTooLong -> Studio.createErrorNameTooLong
        |> showMessage

    /// Shows a prompt that asks the user if they want to release an album and
    /// handles the release.
    let promptToReleaseAlbum band unreleasedAlbum =
        let (UnreleasedAlbum album) = unreleasedAlbum

        let state = State.get ()

        let confirmed =
            showConfirmationPrompt (
                Studio.commonPromptReleaseAlbum album.Name album.Type
            )

        if confirmed then
            releaseAlbum state band unreleasedAlbum |> Duets.Cli.Effect.apply
