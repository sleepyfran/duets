module Duets.Data.World.Cities.Prague.Hospitals

open Duets.Entities

let addGeneralUniversityHospital zone =
    let lobby =
        World.Node.create 0 RoomType.Lobby
        
    let roomGraph = World.Graph.from lobby
    
    let place =
        World.Place.create
            ("734504a7-c994-40f0-bb0e-70d398f0798a" |> Identity.from)
            "General University Hospital"
            65<quality>
            Hospital
            roomGraph
            zone

    World.City.addPlace place
