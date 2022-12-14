module Data.World.Cities.Prague.RehearsalSpaces

open Entities

let addDuetsRehearsalSpace zone =
    let rehearsalSpace = { Price = 55m<dd> }

    let place =
        World.Place.create
            ("e2352c71-f18d-4594-816e-e1780506aa33"
             |> Identity.from)
            "Duets Rehearsal Place"
            20<quality>
            (RehearsalSpace rehearsalSpace)
            zone

    World.City.addPlace place
