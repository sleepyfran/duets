namespace Duets.Entities

[<AutoOpen>]
module BookTypes =
    /// Defines all genres a book can belong to.
    type BookGenre =
        | Fiction
        | NonFiction
        | ScienceFiction
        | Fantasy
        | Mystery
        | Romance
        | Horror
        | Thriller
        | Biography
        | Autobiography
        | SelfHelp
        | History
        | Poetry
        | Drama
        | Comedy
        | Satire
        | Tragedy
        | Epic
        | Technical
    
    /// Defines the effect that reading a specific book has on a character.
    type BookEffect =
        | SkillGain of skill: SkillId * amount: int
        | MoodletGain of
            moodlet: MoodletType *
            expiration: MoodletExpirationTime

    /// Defines a book that can be purchased and read.
    type Book =
        { Title: string
          Author: string
          Genre: BookGenre
          BookEffects: BookEffect list
          ReadProgress: int<percent> }
