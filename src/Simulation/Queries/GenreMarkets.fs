namespace Simulation.Queries

module GenreMarkets =
    open Aether
    open Entities
    open Simulation

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
