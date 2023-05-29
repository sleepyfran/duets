module rec Duets.Data.World.Cities.Madrid.Root

open Duets.Entities

let generate () =
    let createMadrid = World.City.create Madrid 2.3

    let arganzuela =
        { Id = "59771ed6-2cfb-4ec4-a5e8-23a93673b804" |> Identity.from |> ZoneId
          Name = "Arganzuela" }

    let arguelles =
        { Id = "743e96a2-09df-4081-a191-54c4305b1a75" |> Identity.from |> ZoneId
          Name = "Argüelles" }

    let barajas =
        { Id = "59d311af-6564-48a8-857f-8fe9f46bd422" |> Identity.from |> ZoneId
          Name = "Barajas" }

    let embajadores =
        { Id = "da7b5d1f-ba48-4b12-a636-2e07b220b649" |> Identity.from |> ZoneId
          Name = "Embajadores" }

    let justicia =
        { Id = "f9312427-2795-4980-a6b0-887d013245a7" |> Identity.from |> ZoneId
          Name = "Justicia" }

    let sol =
        { Id = "e1d1e975-c92d-468f-81d2-c4745b2994ab" |> Identity.from |> ZoneId
          Name = "Sol" }

    let palacio =
        { Id = "466cd431-dfb7-4e99-b649-5822b73caf19" |> Identity.from |> ZoneId
          Name = "Palacio" }

    let puenteDeVallecas =
        { Id = "13468cbf-92aa-467b-b183-32dd3e22fdd8" |> Identity.from |> ZoneId
          Name = "Puente de Vallecas" }

    let universidad =
        { Id = "44c619c0-1bc1-43c4-92c3-1d485fe0f2b5" |> Identity.from |> ZoneId
          Name = "Universidad" }

    createHome arganzuela
    |> createMadrid
    |> addAirport barajas
    |> Bars.addWrongWay universidad
    |> Cafes.addAmbuCoffee universidad
    |> Cafes.addMisionCafe universidad
    |> ConcertSpaces.addBut justicia
    |> ConcertSpaces.addCaracol embajadores
    |> ConcertSpaces.addMon arguelles
    |> ConcertSpaces.addNuevoApolo embajadores
    |> ConcertSpaces.addShoko palacio
    |> ConcertSpaces.addWurlitzer sol
    |> RehearsalSpaces.addJackOnTheRocks puenteDeVallecas
    |> RehearsalSpaces.addPandorasVox arganzuela
    |> Restaurants.addSumo universidad
    |> Restaurants.addHonestGreens universidad
    |> Studios.addCasaSonora sol
    |> Studios.addRobinGroove universidad
    |> Studios.addTheMetalFactory arganzuela

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
        ("b6246716-5340-4dd2-b095-c8129d3a1cb3" |> Identity.from)
        "Aeropuerto Adolfo Suárez Madrid-Barajas"
        90<quality>
        Airport
        zone
    |> World.City.addPlace
