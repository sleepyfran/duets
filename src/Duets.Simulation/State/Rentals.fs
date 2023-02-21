module Duets.Simulation.State.Rentals

open Aether
open Duets.Entities

let add (rental: Rental) =
    let addRental = Map.add rental.Coords rental
    Optic.map Lenses.State.rentals_ addRental

let remove (rental: Rental) =
    let removeRental = Map.remove rental.Coords
    Optic.map Lenses.State.rentals_ removeRental
