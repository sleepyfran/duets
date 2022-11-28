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

    createHome arganzuela
    |> createMadrid
    |> addAirport barajas

let createHome zone =
    { Id =
        "6ef3c1ab-dec4-44ea-ac95-f53eff3a1c58"
        |> Identity.from
        |> PlaceId
      Name = "Home"
      Quality = 100<quality>
      Type = Home
      Zone = zone }

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
