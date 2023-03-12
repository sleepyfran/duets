namespace Duets.Simulation.SocialNetworks

open Duets.Entities
open Duets.Simulation

module Account =
    /// Creates a new account for the given type (character or band) with the
    /// specified handle.
    let signUp socialNetworkKey accountId handle =
        let account = SocialNetwork.Account.createEmpty accountId handle

        [ SocialNetworkAccountCreated(socialNetworkKey, account)
          SocialNetworkCurrentAccountChanged(socialNetworkKey, account.Id) ]

    /// Generates an effect that flips between the character and the band's account
    /// depending on the currently active account. Assumes that there's always going
    /// to be a second account available for switching, otherwise does nothing.
    let switch state socialNetworkKey =
        let playableCharacter = Queries.Characters.playableCharacter state
        let currentBand = Queries.Bands.currentBand state

        let currentAccount =
            Queries.SocialNetworks.currentAccount state socialNetworkKey

        let allAccounts =
            Queries.SocialNetworks.allAccounts state socialNetworkKey

        match currentAccount with
        | Some account ->
            let updatedAccountId =
                match account.Id with
                | SocialNetworkAccountId.Character _ ->
                    SocialNetworkAccountId.Band currentBand.Id
                | SocialNetworkAccountId.Band _ ->
                    SocialNetworkAccountId.Character playableCharacter.Id

            if Map.containsKey updatedAccountId allAccounts then
                [ SocialNetworkCurrentAccountChanged(
                      socialNetworkKey,
                      updatedAccountId
                  ) ]
            else
                []
        | None -> []

module Post =
    /// Adds a new post to the given account, with the defined text.
    let toMastodon state (account: SocialNetworkAccount) text =
        let currentTime = Queries.Calendar.today state
        let post = SocialNetwork.Post.create account.Id currentTime text

        SocialNetworkPost(SocialNetworkKey.Mastodon, post)
