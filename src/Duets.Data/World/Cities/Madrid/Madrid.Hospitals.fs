module Duets.Data.World.Cities.Madrid.Hospitals

open Duets.Entities

let addGregorioMarañónHospital zone =
    let lobby = World.Node.create 0 RoomType.Lobby

    let roomGraph = World.Graph.from lobby

    let place =
        World.Place.create
            ("59c9f83d-87cb-43e5-9ac6-8a9209026483" |> Identity.from)
            "Hospital General Universitario Gregorio Marañón"
            73<quality>
            Hospital
            roomGraph
            zone

    World.City.addPlace place
