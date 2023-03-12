namespace Duets.Cli.Scenes.Phone.Apps.Mastodon.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module SwitchAccountCommand =
    /// Command to switch between the character and the band's account.
    let create mastodonApp =
        { Name = "switch account"
          Description =
            "Switches between your character's and your band's account"
          Handler =
            fun _ ->
                SocialNetworks.Account.switch
                    (State.get ())
                    SocialNetworkKey.Mastodon
                |> Effect.applyMultiple

                mastodonApp () }
