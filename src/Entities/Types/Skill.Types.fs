namespace Entities

[<AutoOpen>]
module SkillTypes =
    /// Identifier of a skill which represents its internal type.
    type SkillId =
        | Composition
        | Genre of Genre
        | Instrument of InstrumentType
        | MusicProduction

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
