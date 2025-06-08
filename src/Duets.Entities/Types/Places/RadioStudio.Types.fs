namespace Duets.Entities

[<AutoOpen>]
module RadioStudioTypes =
    /// Defines a radio studio in which the band can go get interviewed and
    /// characters can go to work as reporters.
    type RadioStudio =
        {
            /// Genre of music that the radio station plays.
            MusicGenre: Genre
        }
