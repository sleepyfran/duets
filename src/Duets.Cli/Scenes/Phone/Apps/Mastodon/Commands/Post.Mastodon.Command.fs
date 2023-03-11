namespace Duets.Cli.Scenes.Phone.Apps.Mastodon.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Entities
open Duets.Cli.Text
open Duets.Simulation

[<RequireQualifiedAccess>]
module PostCommand =
    /// Command which allows the player to post a new toot.
    let create account mastodonApp =
        { Name = "post"
          Description =
            $"""Allows you to post something new on your current account"""
          Handler =
            fun _ ->
                let tootText =
                    showTextPrompt
                        $"What's on your mind? Will be posted as {Styles.highlight account.Handle}"

                SocialNetworks.postToMastodon (State.get ()) account tootText
                |> Effect.apply

                mastodonApp () }
