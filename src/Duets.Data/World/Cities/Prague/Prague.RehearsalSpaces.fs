module Duets.Data.World.Cities.Prague.RehearsalSpaces

open Duets.Entities
open Duets.Data.World

let addCheapAndFast zone =
    let rehearsalSpace = { Price = 15m<dd> }

    World.Place.create
        ("e2352c71-f18d-4594-816e-e1780506aa33" |> Identity.from)
        "Cheap&Fast Rehearsal Space"
        20<quality>
        (RehearsalSpace rehearsalSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addZkusebnyTovarna zone =
    let rehearsalSpace = { Price = 55m<dd> }

    World.Place.create
        ("1aaa53e5-f949-40c8-a2e2-7a2db64c8e95" |> Identity.from)
        "Zkušebny Praha Továrna"
        78<quality>
        (RehearsalSpace rehearsalSpace)
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
