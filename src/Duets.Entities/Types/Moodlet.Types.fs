namespace Duets.Entities

[<AutoOpen>]
module MoodletTypes =
    /// Defines the time it takes for a moodlet to expire.
    [<RequireQualifiedAccess>]
    type MoodletExpirationTime =
        | Never
        | AfterDayMoments of int<dayMoments>
        | AfterDays of int<days>

    /// Defines all types of moodlet that can be applied to a character.
    [<RequireQualifiedAccess>]
    type MoodletType = NotInspired

    /// Defines a moodlet that can be applied to a character.
    type Moodlet =
        { MoodletType: MoodletType
          StartedOn: Date
          Expiration: MoodletExpirationTime }

    /// Defines a list of moodlets that have been applied to a character.
    type CharacterMoodlets = Set<Moodlet>
