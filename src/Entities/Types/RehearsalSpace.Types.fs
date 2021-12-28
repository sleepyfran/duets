namespace Entities

open Entities

[<AutoOpen>]
module RehearsalSpaceTypes =
    /// Defines a rehearsal space in which the band can go to practice their
    /// songs and compose new ones.
    type RehearsalSpace =
        { Name: string
          Quality: Quality
          Price: Amount }

    /// Defines all types of rooms that can appear inside of a rehearsal space.
    type RehearsalSpaceRoom =
        | Lobby of space: RehearsalSpace
        | Bar of space: RehearsalSpace
        | RehearsalRoom of space: RehearsalSpace
