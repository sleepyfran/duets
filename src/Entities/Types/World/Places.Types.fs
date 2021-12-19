namespace Entities

[<AutoOpen>]
module PlacesTypes =
    /// Defines a rehearsal space in which the band can go to practice their
    /// songs and compose new ones.
    type RehearsalSpace =
        { Name: string
          Quality: Quality
          Price: Amount }

    /// Represents a recording studio where bands can record and produce their
    /// albums before releasing them to the public.
    type Studio =
        { Id: Identity
          Name: string
          Producer: Producer
          PricePerSong: Amount }
