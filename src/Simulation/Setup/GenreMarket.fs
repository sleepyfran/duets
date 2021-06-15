module Simulation.Setup.GenreMarket

open Common
open Entities

let private createGenreMarket randomFloatBetween =
    { MarketPoint = randomFloatBetween 0.1 5.0
      Fluctuation = randomFloatBetween 0.1 1.1 }

/// Creates the genre market by calculating its initial market point and the
/// fluctuation of each of the genres available.
let createGenreMarkets genres =
    List.map
        (fun genre -> (genre, createGenreMarket Random.floatBetween))
        genres
    |> Map.ofList
