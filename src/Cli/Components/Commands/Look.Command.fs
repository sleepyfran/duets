namespace Cli.Components.Commands

open Cli.SceneIndex
open Cli.Text

[<RequireQualifiedAccess>]
module LookCommand =
    let get =
        { Name = "look"
          Description = Command.outDescription
          Handler = (fun _ -> Scene.WorldAfterMovement) }
