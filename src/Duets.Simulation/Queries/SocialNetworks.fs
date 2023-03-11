namespace Duets.Simulation.Queries

open Aether
open Duets.Entities

module SocialNetworks =
    let private socialNetworkByKey state key =
        let socialNetworks = Optic.get Lenses.State.socialNetworks_ state

        match key with
        | SocialNetworkKey.Mastodon -> socialNetworks.Mastodon

    /// Returns all the current accounts for the given social network.
    let allAccounts state key =
        let socialNetwork = socialNetworkByKey state key
        socialNetwork.Accounts

    /// Returns all accounts that can be registered for the current character
    /// and its bands and haven't been yet.
    let unregisteredAccounts state key =
        let character = Characters.playableCharacter state
        let band = Bands.currentBand state
        let allRegisteredAccounts = allAccounts state key

        [ SocialNetworkAccountId.Character character.Id
          SocialNetworkAccountId.Band band.Id ]
        |> List.filter (fun accountId ->
            not (Map.containsKey accountId allRegisteredAccounts))

    /// Returns the current active account of the given social network, if any.
    let currentAccount state key =
        let socialNetwork = socialNetworkByKey state key

        match socialNetwork.CurrentAccount with
        | SocialNetworkCurrentAccountStatus.NoAccountCreated -> None
        | SocialNetworkCurrentAccountStatus.Account id ->
            Map.tryFind id socialNetwork.Accounts

    /// Retrieves the full timeline of posts of a given account.
    let timeline (_: State) (account: SocialNetworkAccount) =
        (* Keeping the unused state so that in the future we can easily modify
        this to actually retrieve more stuff and not just the current account. *)
        account.Posts
