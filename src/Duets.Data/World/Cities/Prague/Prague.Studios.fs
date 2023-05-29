module Duets.Data.World.Cities.Prague.Studios

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let addDuetsStudio zone =
    let producerBirthday = October 2 1996

    let studio =
        { Producer = Character.from "Fran González" Male producerBirthday
          PricePerSong = 250m<dd> }

    World.Place.create
        ("54d72a48-e394-4897-ba3f-dff8941b09df" |> Identity.from)
        "Duets Studio"
        80<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addSmeckyStudios zone =
    let producerBirthday = February 22 1969

    let studio =
        { Producer = Character.from "Jan Holzner" Male producerBirthday
          PricePerSong = 450m<dd> }

    World.Place.create
        ("65bcf4cd-aeb9-4b52-bfc9-bb5c89e8d9a6" |> Identity.from)
        "Smecky Music Studios"
        98<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addStudioFaust zone =
    let producerBirthday = March 12 1978

    let studio =
        { Producer = Character.from "Jaz Coleman" Male producerBirthday
          PricePerSong = 330m<dd> }

    World.Place.create
        ("05616757-5f77-42e7-bd84-0f52b95230f8" |> Identity.from)
        "Studio Faust Records"
        81<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addSquatStudio zone =
    let producerBirthday = November 10 1981

    let studio =
        { Producer = Character.from "Rando Rand" Male producerBirthday
          PricePerSong = 130m<dd> }

    World.Place.create
        ("a17e47b0-cc11-4b8c-a0fc-85a5482a8821" |> Identity.from)
        "Squat Studios"
        57<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
