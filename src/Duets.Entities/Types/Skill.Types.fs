namespace Duets.Entities

[<AutoOpen>]
module SkillTypes =
    /// Identifier of a skill which represents its internal type.
    [<RequireQualifiedAccess>]
    type SkillId =
        (* Music. *)
        | Composition
        | Genre of Genre
        | Instrument of InstrumentType
        (* Production. *)
        | MusicProduction
        (* Character. *)
        | Fitness
        | Speech
        | Cooking
        (* Job. *)
        | Barista
        | Bartending
        | Presenting

    /// Defines all possible categories to which skills can be related to.
    [<RequireQualifiedAccess>]
    type SkillCategory =
        | Character
        | Job
        | Music
        | Production

    /// Represents a skill that the character can have. This only includes the base
    /// fields of the skill, more specific types are available depending on what
    /// information we need.
    type Skill =
        { Id: SkillId; Category: SkillCategory }

    /// Defines the relation between a skill and its level.
    type SkillWithLevel = Skill * int
