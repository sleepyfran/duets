module Duets.Entities.Tests.Band

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities

[<Test>]
let ``Member.validateRoles fails when no roles are given`` () =
    Band.Member.validateRoles []
    |> Result.unwrapError
    |> should be (ofCase <@ Band.NoRoles @>)

[<Test>]
let ``Member.validateRoles fails when roles are not compatible`` () =
    Band.Member.validateRoles [ Guitar; Drums ]
    |> Result.unwrapError
    |> should be (ofCase <@ Band.IncompatibleRoles @>)

[<Test>]
let ``Member.validateRoles succeeds when roles are compatible`` () =
    Band.Member.validateRoles [ Guitar; Vocals ]
    |> Result.unwrap
    |> should equal [ Guitar; Vocals ]
