module Test.Common.Generators.Band

open FsCheck

open Duets.Entities

let generator =
    gen {
        let! initialBand = Arb.generate<Band>
        let! fans = Gen.choose (0, 1000000)

        return { initialBand with Fans = fans }
    }
