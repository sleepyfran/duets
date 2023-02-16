module Duets.Simulation.State.Market

open Aether
open Duets.Entities

let set genreMarkets =
    Optic.set Lenses.State.genreMarkets_ genreMarkets
