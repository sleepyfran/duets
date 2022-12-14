module Data.World.Cities.Prague.Studios

open Fugit.Months
open Entities

let addDuetsStudio zone =
    let producerBirthday = October 2 1996

    let studio =
        { Producer = Character.from "Fran González" Male producerBirthday
          PricePerSong = 250m<dd> }

    let place =
        World.Place.create
            ("54d72a48-e394-4897-ba3f-dff8941b09df" |> Identity.from)
            "Duets Studio"
            80<quality>
            (Studio studio)
            zone

    World.City.addPlace place
