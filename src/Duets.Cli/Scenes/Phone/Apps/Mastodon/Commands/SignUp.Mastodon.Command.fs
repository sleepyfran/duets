namespace Duets.Cli.Scenes.Phone.Apps.Mastodon.Commands

open Duets.Cli.Components.Commands
open Duets.Cli.Scenes.Phone.Apps.Mastodon

[<RequireQualifiedAccess>]
module SignUpCommand =
    /// Command that allows the player to register another account.
    let create mastodonApp =
        { Name = "sign up"
          Description =
            "Allows you to register an account for you or your band, if you haven't done so already"
          Handler = fun _ -> SignUp.showSignUpFlow mastodonApp }
