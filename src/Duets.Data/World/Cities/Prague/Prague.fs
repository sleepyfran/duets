module rec Duets.Data.World.Cities.Prague.Root

open Duets.Entities

let generate () =
    let createPrague = World.City.create Prague 1.8

    let holešovice =
        { Id = "8e17b833-72b3-4aff-af42-62a80363ab45" |> Identity.from |> ZoneId
          Name = "Holešovice" }

    let novéMěsto =
        { Id = "feb334c9-b399-4ff3-83ab-577ddde0f18e" |> Identity.from |> ZoneId
          Name = "Nové Město" }

    let ruzyně =
        { Id = "8a681792-f431-4275-b791-cc5db047bb77" |> Identity.from |> ZoneId
          Name = "Ruzyně" }

    let strašnice =
        { Id = "ed537edd-110d-4088-b47d-21236d0b6133" |> Identity.from |> ZoneId
          Name = "Strašnice" }

    let smichov =
        { Id = "977a09e8-de33-4e62-8cbb-22b2daab30b2" |> Identity.from |> ZoneId
          Name = "Smichov" }

    let vinohrady =
        { Id = "8702f3d8-2d82-4a18-9e2a-fa12338de616" |> Identity.from |> ZoneId
          Name = "Vinohrady" }

    let žižkov =
        { Id = "992937a6-5d65-465d-86f8-e1061a05427f" |> Identity.from |> ZoneId
          Name = "Žižkov" }

    createHome vinohrady
    |> createPrague
    |> addAirport ruzyně
    |> Bars.addBeerGeek vinohrady
    |> Bars.addPubble vinohrady
    |> Cafes.addMamaCoffee vinohrady
    |> Cafes.addTheMiners vinohrady
    |> ConcertSpaces.addDivadloArcha novéMěsto
    |> ConcertSpaces.addFuturum smichov
    |> ConcertSpaces.addKampusHybernska novéMěsto
    |> ConcertSpaces.addLucerna novéMěsto
    |> ConcertSpaces.addPalacAkropolis žižkov
    |> ConcertSpaces.addRedutaJazzClub novéMěsto
    |> ConcertSpaces.addUnderdogs smichov
    |> Hospitals.addGeneralUniversityHospital novéMěsto
    |> RehearsalSpaces.addCheapAndFast ruzyně
    |> RehearsalSpaces.addZkusebnyTovarna strašnice
    |> Restaurants.addChilliAndLime žižkov
    |> Restaurants.addTaiko žižkov
    |> Restaurants.addForkys holešovice
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
