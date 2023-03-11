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

    match currentAccount with
    | Some account -> showPrompt account
    | None -> SignUp.showInitialSignUpFlow mastodonApp

and private showPrompt account =
    let promptText =
        $"""{Emoji.mastodon} {$"@{account.Handle}" |> Styles.highlight} | {$"{Styles.number account.Followers} followers" |> Styles.Level.good}"""

    let appCommands =
        [ Mastodon.Commands.TimelineCommand.create account mastodonApp
          Mastodon.Commands.SignUpCommand.create mastodonApp
          Mastodon.Commands.SwitchAccountCommand.create mastodonApp
          Mastodon.Commands.PostCommand.create account mastodonApp
          Mastodon.Commands.ExitCommand.get ]

    appCommands
    |> (@) [ HelpCommand.createForApp "Mastodon" mastodonApp appCommands ]
    |> showCommandPrompt promptText
