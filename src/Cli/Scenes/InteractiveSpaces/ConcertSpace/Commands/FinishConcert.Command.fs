namespace Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands

open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text

[<RequireQualifiedAccess>]
module EndConcertCommand =
    /// Puts the artist out of the ongoing concert scene, which shows them the
    /// total points accumulated during the concert, the result of it and allows
    /// them to move to other places outside the stage/backstage.
    let rec create ongoingConcert =
        { Name = "end concert"
          Description =
              ConcertText ConcertCommandFinishConcertDescription
              |> I18n.translate
          Handler =
              (fun _ ->
                  // TODO: Implement.
                  Some Scene.World) }
