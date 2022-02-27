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

        let concertSpace =
            { Name = name
              Quality = quality
              Capacity = capacity }

        let lobby =
            ConcertSpaceRoom.Lobby concertSpace
            |> World.Node.create

        return
            World.Place.create concertSpace lobby
            |> ConcertPlace
            |> World.Node.create
    }
