namespace Entities

[<AutoOpen>]
module ConcertSpaceTypes =
    /// Represents a place where the user can have concerts.
    type ConcertSpace =
        { Name: string
          Quality: Quality
          Capacity: int }

    /// Defines all the different rooms that can appear inside of a concert space.
    type ConcertSpaceRoom =
        | Lobby
        | Bar
        | Stage
