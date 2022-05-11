namespace Entities

[<AutoOpen>]
module RehearsalSpaceTypes =
    /// Defines a rehearsal space in which the band can go to practice their
    /// songs and compose new ones.
    type RehearsalSpace =
        { Name: string
          Quality: Quality
          Price: Amount }
