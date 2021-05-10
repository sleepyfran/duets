namespace Entities

open FSharp.Data.UnitSystems.SI.UnitNames

[<AutoOpen>]
module Types =
  /// Defines the type that all entities with an ID should use.
  type Identity = System.Guid

  /// Defines a month in the calendar.
  type Month =
    | January
    | February
    | March
    | April
    | May
    | June
    | July
    | August
    | September
    | October
    | November
    | December

  /// Defines a custom in-game date. All dates in the game start in a year 0 and
  /// go on from there instead of following real life years.
  type Date = { Day: int; Month: Month; Year: int }

  /// Defines a period of time with a start and an optional end.
  type Period = Date * Date

  /// Defines a musical genre. This basic type is just an alias for the name of
  /// the genre, there's more specific types depending on the type of information
  /// that we want to query.
  type Genre = string

  /// Defines the relation between a genre and its popularity in a moment
  /// in time.
  type GenrePopularity = Genre * byte

  /// Defines the percentage compatibility of two genres between 0 and 100.
  type GenreCompatibility = Genre * Genre * byte

  /// Defines the gender of the character.
  type Gender =
    | Male
    | Female
    | Other

  /// Unique identifier of a character.
  type CharacterId = CharacterId of Identity

  /// Defines a character, be it the one that the player is controlling or any
  /// other NPC of the world.
  type Character =
    { Id: CharacterId
      Name: string
      Age: int
      Gender: Gender }

  /// Unique identifier of an instrument.
  type InstrumentId = InstrumentId of Identity

  /// Defines what kind of instrument we're defining to be able to query different
  /// information about it.
  type InstrumentType =
    | Guitar
    | Drums
    | Bass
    | Vocals

  /// Represents the archetype instrument that a character can use.
  type Instrument =
    { Id: InstrumentId
      Type: InstrumentType }

  /// Identifier of a skill which represents its internal type.
  type SkillId =
    | Composition
    | Genre of Genre
    | Instrument of Instrument

  /// Defines all possible categories to which skills can be related to.
  type SkillCategory =
    | Music
    | Production

  /// Represents a skill that the character can have. This only includes the base
  /// fields of the skill, more specific types are available depending on what
  /// information we need.
  type Skill =
    { Id: SkillId
      Category: SkillCategory }

  /// Defines the relation between a skill and its level.
  type SkillWithLevel = Skill * int

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
      Name: string
      Genre: Genre
      Members: CurrentMember list
      PastMembers: PastMember list }

  /// Unique identifier of a song.
  type SongId = SongId of Identity

  /// Defines the different styles of vocals that a song can have.
  type VocalStyle =
    | Instrumental
    | Normal
    | Growl
    | Screamo

  /// Defines a song composed by a band in its most basic form, there's more
  /// specific types depending on the type of information we want to query.
  type Song =
    { Id: SongId
      Name: string
      Length: int<second>
      VocalStyle: VocalStyle
      Genre: Genre }

  /// Defines a song that is still being developed by the band.
  type UnfinishedSong = UnfinishedSong of Song
  /// Defines a song that has been consider finished and it's part of the band's
  /// repertoire and cannot be further developed anymore.
  type FinishedSong = FinishedSong of Song

  /// Measure for the quality of something, as a percentage.
  [<Measure>]
  type quality

  type Quality = int<quality>
  type MaxQuality = int<quality>

  /// Indicates whether the song can be further improved or if it has reached its
  /// maximum quality and thus cannot be improved. All variants wrap an int that
  /// hold the current value.
  type SongStatus =
    | CanBeImproved of Quality
    | ReachedMaxQualityInLastImprovement of Quality
    | ReachedMaxQualityAlready of Quality

  /// Shapes a relation between an unfinished song, its max quality and the
  /// current quality.
  type UnfinishedSongWithQualities = UnfinishedSong * MaxQuality * Quality
  /// Shapes a relation between a finished song and its quality.
  type FinishedSongWithQuality = FinishedSong * Quality


  /// Collection of skills by character.
  type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>

  /// Collection of songs (either finished or unfinished) by a band.
  type SongsByBand<'song> = Map<BandId, Map<SongId, 'song>>

  /// Represents the repertoire of a band with its unfinished and finished songs.
  /// Only finished songs can be recorded and played live.
  type BandRepertoire =
    { Unfinished: SongsByBand<UnfinishedSongWithQualities>
      Finished: SongsByBand<FinishedSongWithQuality> }

  /// Shared state of the game. Contains all state that is common to every part
  /// of the game.
  type State =
    { Bands: Map<BandId, Band>
      Character: Character
      CharacterSkills: CharacterSkills
      CurrentBandId: BandId
      BandRepertoire: BandRepertoire
      Today: Date }

  /// Defines the before and after of an action.
  type Diff<'a> = Diff of before: 'a * after: 'a

  /// Defines an effect that happened after an action in the game. For example
  /// calling composeSong will create a `SongComposed` effect.
  type Effect =
    | GameCreated of State
    | SongStarted of Band * UnfinishedSongWithQualities
    | SongImproved of Band * Diff<UnfinishedSongWithQualities>
    | SongFinished of Band * FinishedSongWithQuality
    | SongDiscarded of Band * UnfinishedSongWithQualities
    | MemberHired of Band * CurrentMember
    | MemberFired of Band * CurrentMember * PastMember
