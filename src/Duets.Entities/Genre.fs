module Duets.Entities.Genre

module Market =
    /// Creates a market for each of the given genres, using the given function
    /// to generate a random float between two values.
    let create (floatBetween: float -> float -> float) (genres: Genre list) =
        genres
        |> List.map (fun genre ->
            genre,
            { MarketPoint = floatBetween 0.1 5.0
              Fluctuation = floatBetween 0.1 1.1 })
        |> Map.ofList

    /// Adds a newly created market for each of the given genres that doesn't
    /// have one yet, keeping the existing ones as they are.
    let addMissing
        floatBetween
        (genres: Genre list)
        (genreMarket: GenreMarketByGenre)
        =
        genres
        |> List.filter (fun genre -> not (Map.containsKey genre genreMarket))
        |> create floatBetween
        |> Map.fold (fun acc genre market -> Map.add genre market acc) genreMarket
