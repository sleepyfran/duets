module Duets.Entities.Tests.Instrument

open FsUnit
open NUnit.Framework

open Duets.Entities

[<Test>]
let ``compatible returns true for a single instrument`` () =
    Instrument.compatible [ Drums ] |> should equal true

[<Test>]
let ``compatible returns true for instruments that use different limbs`` () =
    Instrument.compatible [ Guitar; Vocals ] |> should equal true
    Instrument.compatible [ Bass; Vocals ] |> should equal true
    Instrument.compatible [ Drums; Vocals ] |> should equal true

[<Test>]
let ``compatible returns false for instruments that share a limb`` () =
    Instrument.compatible [ Guitar; Bass ] |> should equal false
    Instrument.compatible [ Guitar; Drums ] |> should equal false

[<Test>]
let ``compatible returns false for the same instrument twice`` () =
    Instrument.compatible [ Vocals; Vocals ] |> should equal false
