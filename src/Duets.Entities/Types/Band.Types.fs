namespace Duets.Entities

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
        { CharacterId: CharacterId
          Role: InstrumentType
          Since: Date }

    /// Defines a member that used to be part of the band but is no longer a
    /// member.
    type PastMember =
        { CharacterId: CharacterId
          Role: InstrumentType
          Period: Period }

    /// Number of fans that a band has.
    type Fans = int

    /// Represents any band inside the game, be it one that is controlled by the
    /// player or the ones that are created automatically to fill the game world.
    type Band =
        { Id: BandId
          StartDate: Date
          Name: string
          Genre: Genre
          Fans: Fans
          Members: CurrentMember list
          PastMembers: PastMember list }
