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
    [<Measure>]
    type fans

    /// Represents the fanbase of a band by city.
    type FanBaseByCity = Map<CityId, int<fans>>

    /// Represents any band inside the game, be it one that is controlled by the
    /// player or the ones that are created automatically to fill the game world.
    type Band =
        { Id: BandId
          StartDate: Date
          OriginCity: CityId
          Name: string
          Genre: Genre
          Fans: FanBaseByCity
          Members: CurrentMember list
          PastMembers: PastMember list }

    /// Map of bands by their ID. Can include the character's and simulated bands.
    type BandMap = Map<BandId, Band>

    /// Includes all the bands in the game.
    type Bands =
        { Current: BandId
          Character: BandMap
          Simulated: BandMap }
