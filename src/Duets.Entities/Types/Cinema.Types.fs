namespace Duets.Entities

[<AutoOpen>]
module CinemaTypes =
    /// Represents a movie that can be shown at a cinema.
    type Movie = { Title: string; Quality: int }
