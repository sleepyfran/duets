namespace Duets.Simulation.Queries

open Aether
open Duets.Entities

module SocialNetworks =
    let private socialNetworkByKey state key =
        let socialNetworks = Optic.get Lenses.State.socialNetworks_ state

        match key with
        | SocialNetworkKey.Mastodon -> socialNetworks.Mastodon

    /// Returns the current active account of the given social network.
    let currentAccount state key =
        let socialNetwork = socialNetworkByKey state key
        (* TODO: Should we handle the absence of a value? *)
        Map.find socialNetwork.CurrentAccount socialNetwork.Accounts

    /// Retrieves the full timeline of posts of a given account.
    let timeline (_: State) (account: SocialNetworkAccount) =
        (* Keeping the unused state so that in the future we can easily modify
        this to actually retrieve more stuff and not just the current account. *)
        account.Posts
