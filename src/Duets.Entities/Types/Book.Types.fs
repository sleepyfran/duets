namespace Duets.Entities

[<AutoOpen>]
module BookTypes =
    /// Defines the effect that reading a specific book has on a character.
    type BookEffect =
        | SkillGain of skill: SkillId * amount: int
        | MoodletGain of moodlet: MoodletType * expiration: MoodletExpirationTime

    /// Defines a book that can be purchased and read.
    type Book =
        { Title: string
          Author: string
          BookEffects: BookEffect list
          ReadProgress: int<percent> }
