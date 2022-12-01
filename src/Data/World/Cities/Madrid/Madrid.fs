module rec Data.World.Cities.Madrid.Root

open Entities

let generate () =
    let createMadrid = World.City.create Madrid

    let arganzuela =
        { Id =
            "59771ed6-2cfb-4ec4-a5e8-23a93673b804"
            |> Identity.from
            |> ZoneId
          Name = "Arganzuela" }

    let barajas =
        { Id =
            "59d311af-6564-48a8-857f-8fe9f46bd422"
            |> Identity.from
            |> ZoneId
          Name = "Barajas" }

    let justicia =
        { Id =
            "f9312427-2795-4980-a6b0-887d013245a7"
            |> Identity.from
            |> ZoneId
          Name = "Justicia" }

    let sol =
        { Id =
            "e1d1e975-c92d-468f-81d2-c4745b2994ab"
            |> Identity.from
            |> ZoneId
          Name = "Sol" }

    let puenteDeVallecas =
        { Id =
            "13468cbf-92aa-467b-b183-32dd3e22fdd8"
            |> Identity.from
            |> ZoneId
          Name = "Puente de Vallecas" }

    let universidad =
        { Id =
            "44c619c0-1bc1-43c4-92c3-1d485fe0f2b5"
            |> Identity.from
            |> ZoneId
          Name = "Universidad" }

    createHome arganzuela
    |> createMadrid
    |> addAirport barajas
    |> addBut justicia
    |> addWurlitzer sol
    |> addWrongWay universidad
    |> addJackOnTheRocks puenteDeVallecas
    |> addPandorasVox arganzuela

let createHome zone =
    { Id =
        "6ef3c1ab-dec4-44ea-ac95-f53eff3a1c58"
        |> Identity.from
        |> PlaceId
      Name = "Home"
      Quality = 100<quality>
      Type = Home
      Zone = zone }


(* -------- Airports --------- *)
let addAirport zone =
    { Id =
        "b6246716-5340-4dd2-b095-c8129d3a1cb3"
        |> Identity.from
        |> PlaceId
      Name = "Aeropuerto Adolfo Suárez Madrid-Barajas"
      Quality = 90<quality>
      Type = Airport
      Zone = zone }
    |> World.City.addPlace

(* -------- Bars --------- *)
let addWrongWay zone =
    let shop =
        { AvailableItems = Shops.commonPubDrinks @ Shops.commonPubFood
          PriceModifier = 3<multiplier> }

    { Id =
        "1085e199-f5ad-4011-bc7e-4c55dbd4a25e"
        |> Identity.from
        |> PlaceId
      Name = "Wrong Way"
      Quality = 70<quality>
      Type = Bar shop
      Zone = zone }
    |> World.City.addPlace

(* -------- Concert Spaces --------- *)
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

(* -------- Rehearsal Rooms --------- *)
let addJackOnTheRocks zone =
    let rehearsalSpace = { Price = 150<dd> }

    { Id =
        "479ec3de-10ef-41a5-a158-882ef031c125"
        |> Identity.from
        |> PlaceId
      Name = "Jack on the Rocks"
      Quality = 50<quality>
      Type = RehearsalSpace rehearsalSpace
      Zone = zone }
    |> World.City.addPlace

let addPandorasVox zone =
    let rehearsalSpace = { Price = 700<dd> }

    { Id =
        "85ebaab3-3e1c-4c3b-afb2-d6ba2944ab9c"
        |> Identity.from
        |> PlaceId
      Name = "Pandora's Vox"
      Quality = 80<quality>
      Type = RehearsalSpace rehearsalSpace
      Zone = zone }
    |> World.City.addPlace
