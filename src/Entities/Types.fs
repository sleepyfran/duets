namespace Entities

open FSharp.Data.UnitSystems.SI.UnitNames
open Myriad.Plugins

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

  /// Defines the end of a period with either a date or by saying that is still
  /// going on.
  type PeriodEnd =
    | Date of Date
    | Ongoing

  /// Defines a period of time with a start and an optional end.
  type Period = Date * PeriodEnd

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

  /// Unique identifier of a band.
  type BandId = BandId of Identity

  /// Defines all possible roles that a character can take inside a band.
  type MemberRole =
    | Singer
    | Guitarist
    | Bassist
    | Drummer

  /// Relates a specific character with its role in the band for a specific
  /// region of time.
  type Member = Character * MemberRole * Period

  /// Represents any band inside the game, be it one that is controlled by the
  /// player or the ones that are created automatically to fill the game world.
  type Band =
    { Id: BandId
      Name: string
      Genre: Genre
      Members: Member list }

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
      VocalStyle: VocalStyle }


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
    | ReachedMaxQuality of Quality

  /// Shapes a relation between an unfinished song, its max quality and the
  /// current quality.
  type UnfinishedSongWithQualities = UnfinishedSong * MaxQuality * Quality
  /// Shapes a relation between a finished song and its quality.
  type FinishedSongWithQuality = FinishedSong * Quality

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

  /// Collection of skills by character.
  type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>

  /// Collection of songs (either finished or unfinished) by a band.
  type SongsByBand<'song> = Map<BandId, Map<SongId, 'song>>

  /// Represents the repertoire of a band with its unfinished and finished songs.
  /// Only finished songs can be recorded and played live.
  [<Generator.Lenses("lens", "Lenses.Lens")>]
  type BandRepertoire =
    { Unfinished: SongsByBand<UnfinishedSongWithQualities>
      Finished: SongsByBand<FinishedSongWithQuality> }

  /// Shared state of the game. Contains all state that is common to every part
  /// of the game.
  [<Generator.Lenses("lens", "Lenses.Lens")>]
  type State =
    { Bands: Band list
      Character: Character
      CharacterSkills: CharacterSkills
      BandRepertoire: BandRepertoire
      Today: Date }
