module rec Duets.Cli.Scenes.Phone.Apps.Mastodon.Root

open Duets.Agents
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Cli.Components
open Duets.Entities
open Duets.Simulation
open Duets.Cli.Scenes.Phone.Apps

let rec mastodonApp () =
    let currentAccount =
        Queries.SocialNetworks.currentAccount
            (State.get ())
            SocialNetworkKey.Mastodon

    let promptText =
        $"""{Emoji.mastodon} {$"@{currentAccount.Handle}" |> Styles.highlight} | {$"{Styles.number currentAccount.Followers} followers" |> Styles.Level.good}"""

    let appCommands =
        [ Mastodon.Commands.TimelineCommand.create currentAccount mastodonApp
          Mastodon.Commands.SwitchAccountCommand.create mastodonApp
          Mastodon.Commands.PostCommand.create currentAccount mastodonApp
          Mastodon.Commands.ExitCommand.get ]

    appCommands
    |> (@) [ HelpCommand.createForApp "Mastodon" mastodonApp appCommands ]
    |> showCommandPrompt promptText
