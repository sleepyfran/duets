namespace Duets.Cli.Components.Commands

open Duets.Cli.SceneIndex
open Duets.Cli.Text

[<RequireQualifiedAccess>]
module PhoneCommand =
    /// Command which opens the phone of the user.
    let get =
        { Name = "phone"
          Description = Command.phoneDescription
          Handler = fun _ -> Scene.Phone }
