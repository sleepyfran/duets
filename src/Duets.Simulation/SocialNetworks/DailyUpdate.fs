namespace Duets.Simulation.SocialNetworks

open Duets.Common
open Duets.Entities
open Duets.Simulation

module DailyUpdate =
    let private estimatedFollowersNeeded state (account: SocialNetworkAccount) =
        let band = Queries.Bands.currentBand state

        match account.Id with
        | SocialNetworkAccountId.Character _ -> float band.Fans * 0.4
        | SocialNetworkAccountId.Band _ -> float band.Fans * 0.7
        |> Math.ceilToNearest

    let private increaseFollowers accountId current needed =
        let extraFollowers = (needed - current) / 10
        let updatedFollowers = current + extraFollowers

        SocialNetworkAccountFollowersChanged(
            SocialNetworkKey.Mastodon,
            accountId,
            Diff(current, updatedFollowers)
        )

    let private increaseFollowersIfNeeded
        state
        (account: SocialNetworkAccount)
        =
        let followersNeeded = estimatedFollowersNeeded state account

        if account.Followers <= followersNeeded then
            [ increaseFollowers account.Id account.Followers followersNeeded ]
        else
            []

    /// Performs the daily update of all the character's social network accounts,
    /// which checks that each has all the followers they should.
    let dailyUpdate state =
        Queries.SocialNetworks.allAccounts state SocialNetworkKey.Mastodon
        |> List.ofMapValues
        |> List.fold
            (fun acc account -> acc @ increaseFollowersIfNeeded state account)
            []
