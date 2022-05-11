namespace Entities

[<AutoOpen>]
module StudioTypes =
    /// Represents the owner of a studio and the character that eventually
    /// produces the album. Their skills determine the final level of the album.
    type Producer = Character

    /// Unique identifier of a studio.
    type StudioId = StudioId of Identity

    /// Represents a recording studio where bands can record and produce their
    /// albums before releasing them to the public.
    type Studio =
        { Name: string
          Producer: Producer
          PricePerSong: Amount }
