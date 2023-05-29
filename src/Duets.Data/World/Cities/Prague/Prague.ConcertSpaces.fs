module Duets.Data.World.Cities.Prague.ConcertSpaces

open Duets.Entities
open Duets.Data.World

let addDivadloArcha zone =
    let concertSpace = { Capacity = 1200 }

    World.Place.create
        ("5a929c9c-ed46-41dd-adcf-4926ad96b445" |> Identity.from)
        "Divadlo Archa"
        89<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addFuturum zone =
    let concertSpace = { Capacity = 350 }

    World.Place.create
        ("623d8d5e-86e7-4a8e-9616-b813ebda66b1" |> Identity.from)
        "Futurum"
        68<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addKampusHybernska zone =
    let concertSpace = { Capacity = 180 }

    World.Place.create
        ("c69099a8-2a77-4c85-9710-d43cec2cade7" |> Identity.from)
        "Kampus Hybernská"
        71<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

let addLucerna zone =
    let concertSpace = { Capacity = 2500 }

    World.Place.create
        ("9917576c-29c8-4d75-b7f8-8857aa99c141" |> Identity.from)
        "Lucerna Palace"
        91<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace

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

let addUnderdogs zone =
    let concertSpace = { Capacity = 400 }

    World.Place.create
        ("bef7a23b-534b-4890-8a0a-e51fc2b8473c" |> Identity.from)
        "Underdogs'"
        64<quality>
        (ConcertSpace concertSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.concertSpaceOpeningHours
    |> World.City.addPlace
