module Duets.Simulation.Tests.SocialNetworks.DailyUpdate

open Duets.Simulation
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Entities
open Duets.Simulation.SocialNetworks.DailyUpdate

let states min max =
    State.generateN
        { State.defaultOptions with
            BandFansMax = max
            BandFansMin = min }
        100

let statesWithAccounts min max characterFollowers bandFollowers =
    states min max
    |> List.map (fun state ->
        let testCharacterAccount =
            SocialNetwork.Account.create
                (SocialNetworkAccountId.Character dummyCharacter.Id)
                "test"
                characterFollowers

        let testBandAccount =
            SocialNetwork.Account.create
                (SocialNetworkAccountId.Band dummyBand.Id)
                "test_band"
                bandFollowers

        state
        |> State.SocialNetworks.addAccount
            SocialNetworkKey.Mastodon
            testCharacterAccount
        |> State.SocialNetworks.addAccount
            SocialNetworkKey.Mastodon
            testBandAccount)

let checkDiffIsGreaterThanOrEqualTo n effect =
    match effect with
    | SocialNetworkAccountFollowersChanged (_, _, Diff (prev, curr)) ->
        curr - prev |> should be (greaterThanOrEqualTo n)
    | _ -> failwith "Unrecognized effect"

[<Test>]
let ``dailyUpdate should return empty list if no accounts created`` () =
    dailyUpdate dummyState |> should haveLength 0

[<Test>]
let ``dailyUpdate should return empty list if account should not have new followers``
    ()
    =
    statesWithAccounts 100 1000 401 701
    |> List.iter (fun state -> dailyUpdate state |> should haveLength 0)

[<Test>]
let ``dailyUpdate increases followers by a tenth if needed`` () =
    statesWithAccounts 500 1000 100 100
    |> List.iter (fun state ->
        let result = dailyUpdate state
        result |> List.head |> checkDiffIsGreaterThanOrEqualTo 10
        result |> List.item 1 |> checkDiffIsGreaterThanOrEqualTo 25)
