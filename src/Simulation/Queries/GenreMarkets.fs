namespace Simulation.Queries

module GenreMarkets =
    open Aether
    open Entities

    /// Returns the genre market of the given genre. If the given genre does not
    /// have a market (maybe the given genre is not valid) it'll throw an
    /// exception.
    let from state genre =
        let genreMarketLens =
            Lenses.FromState.GenreMarkets.genreMarket_ genre

        Optic.get genreMarketLens state |> Option.get
