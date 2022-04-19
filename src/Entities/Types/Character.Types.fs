namespace Entities

open Entities

[<AutoOpen>]
module CharacterTypes =
    /// Defines the gender of the character.
    type Gender =
        | Male
        | Female
        | Other

    /// Unique identifier of a character.
    type CharacterId = CharacterId of Identity

    /// Represents the mood of a character between 0 and 100.
    type Mood = int

    /// Represents the health of a character between 0 and 100.
    type Health = int

    /// Represents the energy of a character between 0 and 100.
    type Energy = int

    /// Gathers all the different needs and statuses of the character that
    /// increase and decrease with the normal flow of the game.
    type CharacterStatus =
        { Mood: Mood
          Health: Health
          Energy: Energy
          Fame: Fame }

    /// Defines a character, be it the one that the player is controlling or any
    /// other NPC of the world.
    type Character =
        { Id: CharacterId
          Name: string
          Age: int
          Gender: Gender
          Status: CharacterStatus }

    /// Collection of skills by character.
    type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
