module rec Duets.Data.World.Cities.Madrid.Root

open Duets.Entities
open Duets.Data.World

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

    let ibiza =
        { Id = "1969ec35-4350-4ea9-a88f-18a96e5a3740" |> Identity.from |> ZoneId
          Name = "Ibiza" }

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
    |> Hospitals.addGregorioMarañónHospital ibiza
    |> RehearsalSpaces.addJackOnTheRocks puenteDeVallecas
    |> RehearsalSpaces.addPandorasVox arganzuela
    |> Restaurants.addSumo universidad
    |> Restaurants.addHonestGreens universidad
    |> Studios.addCasaSonora sol
    |> Studios.addRobinGroove universidad
    |> Studios.addTheMetalFactory arganzuela

(* -------- Home --------- *)
let createHome zone =
    let kitchen = World.Node.create Ids.Home.kitchen RoomType.Kitchen
    let livingRoom = World.Node.create Ids.Home.livingRoom RoomType.LivingRoom
    let bedroom = World.Node.create Ids.Home.bedroom RoomType.Bedroom

    let roomGraph =
        World.Graph.fromMany [ kitchen; livingRoom; bedroom ]
        |> World.Graph.connectMany
            [ kitchen.Id, livingRoom.Id, West; livingRoom.Id, bedroom.Id, East ]

    World.Place.create
        ("6ef3c1ab-dec4-44ea-ac95-f53eff3a1c58" |> Identity.from)
        "Home"
        100<quality>
        Home
        roomGraph
        zone

(* -------- Airport --------- *)
let addAirport zone =
    let lobby = World.Node.create Ids.Airport.lobby RoomType.Lobby

    let securityControl =
        World.Node.create Ids.Airport.securityControl RoomType.SecurityControl

    let roomGraph =
        World.Graph.fromMany [ lobby; securityControl ]
        |> World.Graph.connect lobby.Id securityControl.Id North

    World.Place.create
        ("b6246716-5340-4dd2-b095-c8129d3a1cb3" |> Identity.from)
        "Aeropuerto Adolfo Suárez Madrid-Barajas"
        90<quality>
        Airport
        roomGraph
        zone
    |> World.City.addPlace
