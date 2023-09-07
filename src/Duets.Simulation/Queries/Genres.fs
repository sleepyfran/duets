namespace Duets.Simulation.Queries

open Aether
open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation

module Genres =
    /// Returns the genre market of the given genre. If the given genre does not
    /// have a market (maybe the given genre is not valid) it'll throw an
    /// exception.
    let from state genre =
        let genreMarketLens = Lenses.FromState.GenreMarkets.genreMarket_ genre

        Optic.get genreMarketLens state |> Option.get

    /// Calculates the useful market of a genre, which basically multiplies
    /// the market point by the default market size.
    let usefulMarketOf state genre =
        let market = from state genre

        market.MarketPoint
        |> (*) (float Config.MusicSimulation.defaultMarketSize)

    /// Returns all the available genres in the game and includes their popularity
    /// as a percentage based off the market point of the genre.
    let allWithPopularity state =
        Genres.all
        |> List.map (fun genre ->
            let market = from state genre

            let popularity =
                ((market.MarketPoint - 0.1) / (5.0 - 0.1)) * 100.0
                |> Math.ceilToNearest

            genre, popularity)
