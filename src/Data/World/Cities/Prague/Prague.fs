module rec Data.World.Cities.Prague.Root

open Fugit.Months
open Entities

let rec generate () =
    let createPrague = World.City.create Prague

    let newTown = {
        Id =
            "feb334c9-b399-4ff3-83ab-577ddde0f18e"
            |> Identity.from
            |> ZoneId
        Name = "Nové Město"
    }

    let vinohrady = {
        Id =
            "8702f3d8-2d82-4a18-9e2a-fa12338de616"
            |> Identity.from
            |> ZoneId
        Name = "Vinohrady"
    }

    createHome vinohrady
    |> createPrague
    |> addDuetsRehearsalSpace vinohrady
    |> addDuetsStudio vinohrady
    |> addGeneralUniversityHospital newTown
    |> addPalacAkropolis vinohrady
    |> addRedutaJazzClub newTown

let createHome zone = {
    Id =
        "6ef3c1ab-dec4-44ea-ac95-f53eff3a1c58"
        |> Identity.from
        |> PlaceId
    Name = "Home"
    Quality = 100<quality>
    Type = Home
    Zone = zone
}

let addDuetsRehearsalSpace zone =
    let rehearsalSpace = { Price = 300<dd> }

    let place =
        World.Place.create
            ("e2352c71-f18d-4594-816e-e1780506aa33"
             |> Identity.from)
            "Duets Rehearsal Place"
            20<quality>
            (RehearsalSpace rehearsalSpace)
            zone

    World.City.addPlace place

let addDuetsStudio zone =
    let producerBirthday = October 2 1996

    let studio = {
        Producer = Character.from "Fran González" Male producerBirthday
        PricePerSong = 1000<dd>
    }

    let place =
        World.Place.create
            ("54d72a48-e394-4897-ba3f-dff8941b09df"
             |> Identity.from)
            "Duets Studio"
            80<quality>
            (Studio studio)
            zone

    World.City.addPlace place

let addGeneralUniversityHospital zone =
    let place =
        World.Place.create
            ("734504a7-c994-40f0-bb0e-70d398f0798a"
             |> Identity.from)
            "General University Hospital"
            65<quality>
            Hospital
            zone

    World.City.addPlace place

let addPalacAkropolis zone =
    let concertSpace = { Capacity = 1000 }

    let place =
        World.Place.create
            ("349b0fa9-d5fb-49a6-8a8b-c1513d0627f5"
             |> Identity.from)
            "Palác Akropolis"
            75<quality>
            (ConcertSpace concertSpace)
            zone

    World.City.addPlace place

let addRedutaJazzClub zone =
    let concertSpace = { Capacity = 250 }

    let place =
        World.Place.create
            ("1eb502e6-ebc8-4846-9a07-11c10f962a51"
             |> Identity.from)
            "Reduta Jazz Club"
            95<quality>
            (ConcertSpace concertSpace)
            zone

    World.City.addPlace place
