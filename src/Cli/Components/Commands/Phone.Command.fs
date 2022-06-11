namespace Cli.Components.Commands

open Cli.SceneIndex
open Cli.Text

[<RequireQualifiedAccess>]
module PhoneCommand =
    /// Command which opens the phone of the user.
    let get =
        { Name = "phone"
          Description = Command.phoneDescription
          Handler = fun _ -> Scene.Phone }
