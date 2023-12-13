module Duets.Simulation.Tests.SocialNetworks.Reposts

open NUnit.Framework
open FsUnit
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation

let testAccount =
    SocialNetwork.Account.create
        (SocialNetworkAccountId.Character dummyCharacter.Id)
        "test"
        1450

let testPost = SocialNetwork.Post.create testAccount.Id dummyToday "Heyaaaa"

let checkRepostValueIs value effect =
    match effect with
    | SocialNetworkPostReposted(_, _, reposts) -> reposts |> should equal value
    | _ -> failwith "Unknown effect raised"

let checkFollowerValueIs value effect =
    match effect with
    | SocialNetworkAccountFollowersChanged(_, _, Diff(_, curr)) ->
        curr |> should equal value
    | _ -> failwith "Unknown effect raised"

[<Test>]
let ``should not raise effects if there's no accounts`` () =
    State.generateOne State.defaultOptions
    |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
    |> should haveLength 0

[<Test>]
let ``should not raise effects if there's no posts`` () =
    State.generateOne State.defaultOptions
    |> State.SocialNetworks.addAccount SocialNetworkKey.Mastodon testAccount
    |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
    |> should haveLength 0

[<Test>]
let ``should not raise effects if there's no reposts`` () =
    use _ = changeToStaticRandom 0

    State.generateOne State.defaultOptions
    |> State.SocialNetworks.addAccount SocialNetworkKey.Mastodon testAccount
    |> State.SocialNetworks.addPost SocialNetworkKey.Mastodon testPost
    |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
    |> should haveLength 0

[<Test>]
let ``should not raise effects if there the posts are older than 3 days`` () =
    RandomGen.reset ()

    [ -4; -5; -10 ]
    |> List.iter (fun days ->
        State.generateOne State.defaultOptions
        |> State.SocialNetworks.addAccount
            SocialNetworkKey.Mastodon
            testAccount
        |> State.SocialNetworks.addPost
            SocialNetworkKey.Mastodon
            { testPost with
                Timestamp = dummyToday |> Calendar.Ops.addDays days }
        |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
        |> should haveLength 0)

[<Test>]
let ``should generate effect with new repost count`` () =
    [ 1, 2; 2, 3; 3, 5 ]
    |> List.iter (fun (random, expected) ->
        use _ = changeToStaticRandom random

        State.generateOne State.defaultOptions
        |> State.SocialNetworks.addAccount
            SocialNetworkKey.Mastodon
            testAccount
        |> State.SocialNetworks.addPost SocialNetworkKey.Mastodon testPost
        |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
        |> List.head
        |> checkRepostValueIs expected)

[<Test>]
let ``should generate effect with new follower count`` () =
    [ 1, 145015; 2, 145029; 3, 145044 ]
    |> List.iter (fun (random, expected) ->
        use _ = changeToStaticRandom random

        State.generateOne State.defaultOptions
        |> State.SocialNetworks.addAccount
            SocialNetworkKey.Mastodon
            { testAccount with Followers = 145000 }
        |> State.SocialNetworks.addPost SocialNetworkKey.Mastodon testPost
        |> SocialNetworks.Reposts.applyToLatestAfterTimeChange
        |> List.item 1
        |> checkFollowerValueIs expected)
