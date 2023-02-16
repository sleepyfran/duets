module Duets.Data.World.Cities.Prague.Hospitals

open Duets.Entities

let addGeneralUniversityHospital zone =
    let place =
        World.Place.create
            ("734504a7-c994-40f0-bb0e-70d398f0798a" |> Identity.from)
            "General University Hospital"
            65<quality>
            Hospital
            zone

    World.City.addPlace place
