module Duets.Data.World.Cities.Madrid.ConcertSpaces

open Duets.Entities
open Duets.Data.World

let addBut zone =
    let concertSpace = { Capacity = 1000 }

    World.Place.create
        ("81e86a20-0272-4a8a-8da9-36bb7c0e570c" |> Identity.from)
        "Sala But"
        95<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addWurlitzer zone =
    let concertSpace = { Capacity = 170 }

    World.Place.create
        ("a24fbb24-edf5-42cb-a1fe-d00d3506f147" |> Identity.from)
        "Wurlitzer Ballroom"
        85<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace
