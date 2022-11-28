module Simulation.State.Flights

open Aether
open Entities

let addBooking flight =
    Optic.map Lenses.State.flights_ (List.append [ flight ])
