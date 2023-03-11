namespace Duets.Cli.Scenes.Phone.Apps.Mastodon.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Entities

[<RequireQualifiedAccess>]
module TimelineCommand =
    /// Command which shows the current timeline to the player.
    let create account mastodonApp =
        { Name = "timeline"
          Description = "Shows all the toots that you previously posted"
          Handler =
            fun _ ->
                if List.isEmpty account.Posts then
                    "Nothing to show" |> showMessage
                else
                    account.Posts |> List.iter (showPost account)

                mastodonApp () }
