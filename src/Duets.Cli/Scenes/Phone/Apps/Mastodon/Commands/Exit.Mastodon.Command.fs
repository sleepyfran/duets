namespace Duets.Cli.Scenes.Phone.Apps.Mastodon.Commands

open Duets.Cli.SceneIndex
open Duets.Cli.Components.Commands

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which returns the user to the phone.
    let get =
        { Name = "exit"
          Description = "Closes the app and returns you to the phone"
          Handler = fun _ -> Scene.Phone }
