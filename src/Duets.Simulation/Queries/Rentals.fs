namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Entities

module Rentals =
    /// Returns an optional rental for a place given its coordinates.
    let getForCoords state coords =
        Optic.get (Lenses.State.rentals_ >-> Map.value_ coords) state
