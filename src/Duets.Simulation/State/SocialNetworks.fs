module Duets.Simulation.State.SocialNetworks

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let socialNetworkLens_ id =
    let lens =
        match id with
        | SocialNetworkKey.Mastodon -> Lenses.SocialNetworks.mastodon_

    Lenses.State.socialNetworks_ >-> lens

let addPost id (post: SocialNetworkPost) =
    let lens =
        socialNetworkLens_ id
        >-> Lenses.SocialNetwork.accounts_
        >-> Map.key_ post.AccountId
        >?> Lenses.SocialNetwork.Account.posts_

    Optic.map lens (fun posts -> post :: posts)

let switchAccount id account =
    let lens = socialNetworkLens_ id

    Optic.map lens (fun socialNetwork ->
        { socialNetwork with CurrentAccount = account })
