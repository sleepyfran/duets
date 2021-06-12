module Cli.View.Scenes.Studio.PromptToRelease

open Cli.View.Actions
open Cli.View.TextConstants
open Entities
open Simulation.Studio.ReleaseAlbum

/// Shows a prompt that asks the user if they want to release an album and
/// handles the release.
let rec promptToReleaseAlbum onCancel state studio band unreleasedAlbum =
    let (UnreleasedAlbum album) = unreleasedAlbum

    seq {
        yield
            Prompt
                { Title =
                      TextConstant
                      <| StudioCommonPromptReleaseAlbum album.Name
                  Content =
                      ConfirmationPrompt
                      <| handleReleaseConfirmation
                          onCancel
                          state
                          studio
                          band
                          unreleasedAlbum }
    }

and handleReleaseConfirmation onCancel state studio band album confirmed =
    seq {
        if confirmed then
            yield!
                Simulation.Galactus.runOne state (releaseAlbum state band album)
                |> Seq.map Effect

            yield SceneAfterKey <| Studio studio
        else
            yield! onCancel
    }
