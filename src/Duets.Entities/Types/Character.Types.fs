namespace Duets.Entities

open Duets.Entities

[<AutoOpen>]
module CharacterTypes =
    /// Defines the gender of the character.
    type Gender =
        | Male
        | Female
        | Other

    /// Unique identifier of a character.
    type CharacterId = CharacterId of Identity

    /// Identifier of an attribute of a character.
    [<RequireQualifiedAccess>]
    type CharacterAttribute =
        | Drunkenness
        | Energy
        | Fame
        | Health
        | Hunger
        | Mood

    /// Wraps an int to define the amount of an attribute. These are always
    /// between 0 and 100.
    type CharacterAttributeAmount = int

    /// Gathers all the different needs and statuses of the character that
    /// increase and decrease with the normal flow of the game. All these attributes
    /// are represented from 0 to 100 and the absence of an attribute in the
    /// map indicates that the value is 0.
    type CharacterAttributes = Map<CharacterAttribute, CharacterAttributeAmount>

    /// Defines a character, be it the one that the player is controlling or any
    /// other NPC of the world.
    type Character =
        { Id: CharacterId
          Name: string
          Birthday: Date
          Gender: Gender
          Attributes: CharacterAttributes }

    /// Collection of skills by character.
    type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
