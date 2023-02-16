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
