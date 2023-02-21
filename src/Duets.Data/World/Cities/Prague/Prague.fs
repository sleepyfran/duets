module rec Duets.Data.World.Cities.Prague.Root

open Duets.Entities

let generate () =
    let createPrague = World.City.create Prague 1.8

    let newTown =
        { Id = "feb334c9-b399-4ff3-83ab-577ddde0f18e" |> Identity.from |> ZoneId
          Name = "Nové Město" }

    let ruzyne =
        { Id = "8a681792-f431-4275-b791-cc5db047bb77" |> Identity.from |> ZoneId
          Name = "Ruzyně" }

    let vinohrady =
        { Id = "8702f3d8-2d82-4a18-9e2a-fa12338de616" |> Identity.from |> ZoneId
          Name = "Vinohrady" }

    let zizkov =
        { Id = "992937a6-5d65-465d-86f8-e1061a05427f" |> Identity.from |> ZoneId
          Name = "Žižkov" }

    createHome vinohrady
    |> createPrague
    |> addAirport ruzyne
    |> Bars.addBeerGeek vinohrady
    |> Bars.addPubble vinohrady
    |> Cafes.addMamaCoffee vinohrady
    |> Cafes.addTheMiners vinohrady
    |> ConcertSpaces.addPalacAkropolis zizkov
    |> ConcertSpaces.addRedutaJazzClub newTown
    |> Hospitals.addGeneralUniversityHospital newTown
    |> RehearsalSpaces.addDuetsRehearsalSpace vinohrady
    |> Restaurants.addChilliAndLime zizkov
    |> Restaurants.addTaiko zizkov
    |> Studios.addDuetsStudio vinohrady

(* -------- Home --------- *)
let createHome zone =
    World.Place.create
        ("6ef3c1ab-dec4-44ea-ac95-f53eff3a1c58" |> Identity.from)
        "Home"
        100<quality>
        Home
        zone

(* -------- Airport --------- *)
let addAirport zone =
    World.Place.create
        ("93e39c34-08a0-41b5-8112-78f0aa2de279" |> Identity.from)
        "Letiště Václava Havla Praha"
        85<quality>
        Airport
        zone
    |> World.City.addPlace
