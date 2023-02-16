module Duets.Simulation.State.Flights

open Aether
open Duets.Entities

let addBooking flight =
    Optic.map Lenses.State.flights_ (List.append [ flight ])

let change (updatedFlight: Flight) =
    Optic.map
        Lenses.State.flights_
        (List.map (fun flight ->
            if flight.Id = updatedFlight.Id then
                updatedFlight
            else
                flight))
