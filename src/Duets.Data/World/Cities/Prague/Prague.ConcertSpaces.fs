module Duets.Data.World.Cities.Prague.ConcertSpaces

open Duets.Entities
open Duets.Data.World

let addPalacAkropolis zone =
    let concertSpace = { Capacity = 1000 }

    World.Place.create
        ("349b0fa9-d5fb-49a6-8a8b-c1513d0627f5" |> Identity.from)
        "Palác Akropolis"
        75<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addRedutaJazzClub zone =
    let concertSpace = { Capacity = 250 }

    World.Place.create
        ("1eb502e6-ebc8-4846-9a07-11c10f962a51" |> Identity.from)
        "Reduta Jazz Club"
        95<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace
