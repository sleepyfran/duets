module Data.World.Cities.Madrid.RehearsalSpaces

open Entities
open Data.World

let addJackOnTheRocks zone =
    let rehearsalSpace = { Price = 20m<dd> }

    World.Place.create
        ("479ec3de-10ef-41a5-a158-882ef031c125" |> Identity.from)
        "Jack on the Rocks"
        50<quality>
        (RehearsalSpace rehearsalSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addPandorasVox zone =
    let rehearsalSpace = { Price = 65m<dd> }

    World.Place.create
        ("85ebaab3-3e1c-4c3b-afb2-d6ba2944ab9c" |> Identity.from)
        "Pandora's Vox"
        80<quality>
        (RehearsalSpace rehearsalSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
