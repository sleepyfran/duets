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
        | Album.NameTooShort -> StudioText StudioCreateErrorNameTooShort
        | Album.NameTooLong -> StudioText StudioCreateErrorNameTooLong
        |> I18n.translate
        |> showMessage

    /// Shows a prompt that asks the user if they want to release an album and
    /// handles the release.
    let rec promptToReleaseAlbum band unreleasedAlbum =
        let (UnreleasedAlbum album) =
            unreleasedAlbum

        let state = State.get ()

        let confirmed =
            showConfirmationPrompt (
                StudioCommonPromptReleaseAlbum album.Name
                |> StudioText
                |> I18n.translate
            )

        if confirmed then
            releaseAlbum state band unreleasedAlbum
            |> Cli.Effect.apply
