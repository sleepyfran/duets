namespace Entities

[<AutoOpen>]
module CharacterTypes =
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

    /// Collection of skills by character.
    type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
