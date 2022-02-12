module Cli.Scenes.Studio.PromptToRelease

open Agents
open Cli.Actions
open Cli.Text
open Entities
open Simulation.Studio.ReleaseAlbum

/// Shows a prompt that asks the user if they want to release an album and
/// handles the release.
let rec promptToReleaseAlbum onCancel studio band unreleasedAlbum =
    let (UnreleasedAlbum album) = unreleasedAlbum

    seq {
        yield
            Prompt
                { Title =
                      StudioCommonPromptReleaseAlbum album.Name
                      |> StudioText
                      |> I18n.translate
                  Content =
                      ConfirmationPrompt
                      <| handleReleaseConfirmation
                          onCancel
                          studio
                          band
                          unreleasedAlbum }
    }

and handleReleaseConfirmation onCancel studio band album confirmed =
    let state = State.get ()

    seq {
        if confirmed then
            yield releaseAlbum state band album |> Effect

            yield Scene Scene.World
        else
            yield! onCancel
    }
