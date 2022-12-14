module Data.World.Cities.Madrid.ConcertSpaces

open Entities

let addBut zone =
    let concertSpace = { Capacity = 1000 }

    { Id =
        "81e86a20-0272-4a8a-8da9-36bb7c0e570c"
        |> Identity.from
        |> PlaceId
      Name = "Sala But"
      Quality = 95<quality>
      Type = ConcertSpace concertSpace
      Zone = zone }
    |> World.City.addPlace

let addWurlitzer zone =
    let concertSpace = { Capacity = 170 }

    { Id =
        "a24fbb24-edf5-42cb-a1fe-d00d3506f147"
        |> Identity.from
        |> PlaceId
      Name = "Wurlitzer Ballroom"
      Quality = 85<quality>
      Type = ConcertSpace concertSpace
      Zone = zone }
    |> World.City.addPlace
