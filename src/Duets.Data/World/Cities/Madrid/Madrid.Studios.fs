module Duets.Data.World.Cities.Madrid.Studios

open Fugit.Months
open Duets.Entities
open Duets.Data.World

let addCasaSonora zone =
    let producerBirthday = May 12 1979

    let studio =
        { Producer = Character.from "Pablo Sciuto" Male producerBirthday
          PricePerSong = 400m<dd> }

    World.Place.create
        ("3c4d79ae-88ad-4e54-b864-9c7fe9ba997d" |> Identity.from)
        "Casa Sonora"
        98<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addRobinGroove zone =
    let producerBirthday = October 10 1984

    let studio =
        { Producer = Character.from "Robin Grooves" Male producerBirthday
          PricePerSong = 300m<dd> }

    World.Place.create
        ("b7549a68-e1eb-4fcc-bcf4-d51eb47361ec" |> Identity.from)
        "Robin Groove"
        76<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addTheMetalFactory zone =
    let producerBirthday = February 20 1989

    let studio =
        { Producer = Character.from "Alex Cappa" Male producerBirthday
          PricePerSong = 300m<dd> }

    World.Place.create
        ("3c4d79ae-88ad-4e54-b864-9c7fe9ba997d" |> Identity.from)
        "The Metal Factory"
        89<quality>
        (Studio studio)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
