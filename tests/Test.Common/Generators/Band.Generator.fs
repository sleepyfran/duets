module Test.Common.Generators.Band

open FsCheck

open Entities

let generator =
    gen {
        let! initialBand = Arb.generate<Band>
        let! fame = Gen.choose (0, 100)

        return { initialBand with Fame = fame }
    }
