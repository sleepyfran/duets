[<RequireQualifiedAccess>]
module Duets.Simulation.Market.GenreMarket

open Duets.Common
open Duets.Entities

let private updateGenreMarket market =
    { market with
        MarketPoint =
            Random.boolean ()
            |> fun increasing ->
                if increasing then
                    market.MarketPoint + market.Fluctuation
                else
                    market.MarketPoint - market.Fluctuation
            |> Math.clampFloat 2.0 5.0 }

/// Creates the genre market by calculating its initial market point and the
/// fluctuation of each of the genres available.
let create (genres: Genre list) = Genre.Market.create Random.floatBetween genres

/// Updates all the genres of the market by updating its market point to the
/// previously calculated fluctuation. All the market points will be between
/// 2 and 5.
let update (genreMarket: GenreMarketByGenre) =
    genreMarket |> Map.map (fun _ -> updateGenreMarket) |> GenreMarketsUpdated
