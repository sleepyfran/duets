namespace Duets.Simulation.SocialNetworks

open Duets.Common
open Duets.Entities
open Duets.Simulation

module Reposts =
    let private calculateNewReposts account =
        ((float (RandomGen.genBetween 0 3)) * 0.001) * float account.Followers
        |> Math.ceilToNearest

    let private calculateNewFollowers reposts =
        float reposts * 0.1 |> Math.ceilToNearest

    /// Generates reposts for all posts posted from the character and band accounts
    /// in the last 3 days, based on the reach that the post should have in this
    /// specific moment.
    let applyToLatestAfterTimeChange state =
        Queries.SocialNetworks.allAccounts state SocialNetworkKey.Mastodon
        |> List.ofMapValues
        |> List.fold
            (fun effects account ->
                let newReposts = calculateNewReposts account

                (* Only raise effect if there's any reposts. *)
                if newReposts > 0 then
                    Queries.SocialNetworks.postsFromPreviousNDays
                        state
                        account
                        3<days>
                    |> List.collect (fun post ->
                        let updatedReposts = post.Reposts + newReposts

                        let newFollowers =
                            calculateNewFollowers updatedReposts

                        let updatedFollowerCount =
                            account.Followers + newFollowers

                        [ SocialNetworkPostReposted(
                              SocialNetworkKey.Mastodon,
                              post,
                              updatedReposts
                          )
                          SocialNetworkAccountFollowersChanged(
                              SocialNetworkKey.Mastodon,
                              post.AccountId,
                              Diff(account.Followers, updatedFollowerCount)
                          ) ])
                else
                    []
                @ effects)
            []
