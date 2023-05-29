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

let addCaracol zone =
    let concertSpace = { Capacity = 500 }

    World.Place.create
        ("e6af327b-47c5-419c-86c2-f38b6f8dff9a" |> Identity.from)
        "Sala Caracol"
        70<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addMon zone =
    let concertSpace = { Capacity = 1200 }

    World.Place.create
        ("25135408-b6be-4998-9203-291325a0cde6" |> Identity.from)
        "Sala Mon"
        65<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addNuevoApolo zone =
    let concertSpace = { Capacity = 1160 }

    World.Place.create
        ("1c18d810-6f4d-464f-be39-7134a546f5cd" |> Identity.from)
        "Teatro Nuevo Apolo"
        87<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addShoko zone =
    let concertSpace = { Capacity = 900 }

    World.Place.create
        ("d0b15057-762c-455c-a2e8-72cf6b3109ab" |> Identity.from)
        "Shoko"
        80<quality>
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
