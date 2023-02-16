namespace Duets.Entities

[<AutoOpen>]
module GenreTypes =
    /// Defines a musical genre. This basic type is just an alias for the name of
    /// the genre, there's more specific types depending on the type of information
    /// that we want to query.
    type Genre = string

    /// Defines the relation between a genre and its popularity in a moment
    /// in time.
    type GenrePopularity = Genre * byte

    /// Defines the percentage compatibility of two genres between 0 and 100.
    type GenreCompatibility = Genre * Genre * byte

    /// Defines the potential market of a genre by:
    /// - Market point: modifier between 0.1 and 5 that, multiplied by the default
    ///   the market size, gives the total amount of people willing to listen to
    ///   the genre.
    /// - Fluctuation: modifier between 1 and 1.1 that indicates how much the
    ///   market point will vary yearly. This fluctuation can randomly happen
    ///   in a positive or negative way.
    type GenreMarket =
        { MarketPoint: float
          Fluctuation: float }

    /// Defines the genre market by genre.
    type GenreMarketByGenre = Map<Genre, GenreMarket>
