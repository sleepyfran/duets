module Duets.Cli.Scenes.Phone.Apps.Mastodon.SignUp

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

let rec showInitialSignUpFlow mastodonApp =
    "It looks like you don't have any account yet" |> showMessage

    let confirmed =
        "Do you want to sign-up for an account?" |> showConfirmationPrompt

    if confirmed then
        showSignUpFlow mastodonApp
    else
        Scene.Phone

and showSignUpFlow mastodonApp =
    let possibleAccounts =
        Queries.SocialNetworks.unregisteredAccounts
            (State.get ())
            SocialNetworkKey.Mastodon

    if List.isEmpty possibleAccounts then
        "You have already registered all your possible accounts"
        |> Styles.error
        |> showMessage
    else
        askForAccountId possibleAccounts

    mastodonApp ()

and private askForAccountId possibleAccounts =
    let accountType =
        showOptionalChoicePrompt
            "Who do you want to register for?"
            Generic.cancel
            (function
            | SocialNetworkAccountId.Character characterId ->
                let character =
                    Queries.Characters.byId (State.get ()) characterId

                $"For me, {character.Name |> Styles.highlight}"
            | SocialNetworkAccountId.Band bandId ->
                let band = Queries.Bands.byId (State.get ()) bandId
                $"For my band, {band.Name |> Styles.highlight}")
            possibleAccounts

    match accountType with
    | Some id -> askForHandle id
    | None -> ()

and private askForHandle accountId =
    let handle = "What will the account handle be?" |> showTextPrompt

    SocialNetworks.Account.signUp SocialNetworkKey.Mastodon accountId handle
    |> Effect.applyMultiple

    "Account created!" |> Styles.success |> showMessage
