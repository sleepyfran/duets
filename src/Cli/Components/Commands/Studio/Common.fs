namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.Text
open Entities
open Simulation
open Simulation.Studio.ReleaseAlbum

module Studio =
    /// Shows an error indicating what made the album name validation fail.
    let showAlbumNameError error =
        match error with
        | Album.NameTooShort -> Studio.createErrorNameTooShort
        | Album.NameTooLong -> Studio.createErrorNameTooLong
        |> showMessage

    /// Shows a prompt that asks the user if they want to release an album and
    /// handles the release.
    let rec promptToReleaseAlbum band unreleasedAlbum =
        let (UnreleasedAlbum album) =
            unreleasedAlbum

        let state = State.get ()

        let confirmed =
            showConfirmationPrompt (Studio.commonPromptReleaseAlbum album.Name)

        if confirmed then
            releaseAlbum state band unreleasedAlbum
            |> Cli.Effect.apply
