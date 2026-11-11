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

[<Test>]
let ``family groups guitar and bass as strings`` () =
    Instrument.family Guitar |> should equal Strings
    Instrument.family Bass |> should equal Strings

[<Test>]
let ``compatible allows playing the harmonica with a guitar or keyboard`` () =
    Instrument.compatible [ Guitar; Harmonica ] |> should equal true
    Instrument.compatible [ Keyboard; Harmonica ] |> should equal true

[<Test>]
let ``compatible does not allow singing while playing a mouth instrument`` () =
    Instrument.compatible [ Harmonica; Vocals ] |> should equal false
    Instrument.compatible [ Saxophone; Vocals ] |> should equal false
    Instrument.compatible [ Trumpet; Vocals ] |> should equal false

[<Test>]
let ``compatible does not allow two instruments that need hands`` () =
    Instrument.compatible [ Guitar; Keyboard ] |> should equal false
    Instrument.compatible [ Saxophone; Turntables ] |> should equal false

[<Test>]
let ``compatible allows singing while playing keys or turntables`` () =
    Instrument.compatible [ Keyboard; Vocals ] |> should equal true
    Instrument.compatible [ Turntables; Vocals ] |> should equal true
