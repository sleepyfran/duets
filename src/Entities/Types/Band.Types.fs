namespace Entities

[<AutoOpen>]
module BandTypes =
    /// Unique identifier of a band.
    type BandId = BandId of Identity

    /// Defines a member that is available for being hired into the band.
    type MemberForHire =
        { Character: Character
          Role: InstrumentType
          Skills: SkillWithLevel list }

    /// Defines a current member of the band.
    type CurrentMember =
        { Character: Character
          Role: InstrumentType
          Since: Date }

    /// Defines a member that used to be part of the band but is no longer a
    /// member.
    type PastMember =
        { Character: Character
          Role: InstrumentType
          Period: Period }

    /// Represents any band inside the game, be it one that is controlled by the
    /// player or the ones that are created automatically to fill the game world.
    type Band =
        { Id: BandId
          StartDate: Date
          Name: string
          Genre: Genre
          Fame: Fame
          Members: CurrentMember list
          PastMembers: PastMember list }
