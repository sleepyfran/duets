module Simulation.State.Market

open Aether
open Entities

let set genreMarkets =
    Optic.set Lenses.State.genreMarkets_ genreMarkets
