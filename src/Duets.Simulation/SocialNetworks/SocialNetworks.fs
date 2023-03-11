module Duets.Simulation.SocialNetworks

open Duets.Entities
open Duets.Simulation

/// Generates an effect that flips between the character and the band's account
/// depending on the currently active account.
let switchAccount state socialNetworkKey =
    let playableCharacter = Queries.Characters.playableCharacter state
    let currentBand = Queries.Bands.currentBand state

    let currentAccount =
        Queries.SocialNetworks.currentAccount state socialNetworkKey

    let updatedAccount =
        match currentAccount.Id with
        | SocialNetworkAccountId.Character _ ->
            SocialNetworkAccountId.Band currentBand.Id
        | SocialNetworkAccountId.Band _ ->
            SocialNetworkAccountId.Character playableCharacter.Id

    SocialNetworkAccountChanged(socialNetworkKey, updatedAccount)

/// Adds a new post to the given account, with the defined text.
let postToMastodon state (account: SocialNetworkAccount) text =
    let currentTime = Queries.Calendar.today state
    let post = SocialNetwork.Post.create account.Id currentTime text

    SocialNetworkPost(SocialNetworkKey.Mastodon, post)
