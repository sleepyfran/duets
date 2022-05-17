module Test.Common.Generators.City

open FsCheck

open Entities

let venueGenerator =
    gen {
        let! name =
            Arb.generate<NonNull<string>>
            |> Gen.map (fun (NonNull str) -> str)

        let! quality = Gen.choose (0, 100) |> Gen.map ((*) 1<quality>)
        let! capacity = Gen.choose (100, 15000)

        let concertSpace = { Capacity = capacity }

        let lobby = Room.Lobby |> World.Node.create

        return
            World.Place.create name quality (ConcertSpace concertSpace) lobby
            |> CityNode.Place
            |> World.Node.create
    }
