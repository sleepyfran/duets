[<AutoOpen>]
module Duets.Cli.Components.Post

open Duets.Cli.Text
open Duets.Entities
open Spectre.Console

/// Shows a social network post posted by the given account.
let showPost (account: SocialNetworkAccount) (post: SocialNetworkPost) =
    Panel(
        Rows(
            Markup($"@{account.Handle}" |> Styles.faded),
            Text(post.Text),
            Markup($"{Emoji.boost} {post.Reposts}")
        ),
        Border = BoxBorder.Rounded,
        Expand = true
    )
    |> AnsiConsole.Write
