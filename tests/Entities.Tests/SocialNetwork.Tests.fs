module Duets.Entities.Tests.SocialNetwork

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities

[<Test>]
let ``createEmpty should remove ats in the beginning of the handle`` () =
    [ "@test", "test"; "test@", "test@" ]
    |> List.iter (fun (input, expected) ->
        let account =
            SocialNetwork.Account.createEmpty
                (SocialNetworkAccountId.Band dummyBand.Id)
                input

        account.Handle |> should equal expected)
