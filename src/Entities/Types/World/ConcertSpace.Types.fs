namespace Entities

[<AutoOpen>]
module ConcertSpaceTypes =
    /// Represents a place where the user can have concerts.
    type ConcertSpace =
        { Name: string
          Quality: Quality
          Capacity: int }
