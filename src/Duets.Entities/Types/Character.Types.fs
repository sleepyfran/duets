namespace Duets.Entities

[<AutoOpen>]
module CharacterTypes =
    /// Defines the gender of the character.
    type Gender =
        | Male
        | Female
        | Other

    /// Unique identifier of a character.
    type CharacterId = CharacterId of Identity

    /// Conversation topics that a character might like or dislike.
    type ConversationTopic =
        | MusicTheory
        | IndustryGossip
        | Politics
        | Relationships
        | Technology

    /// Likes and dislikes of a character.
    type AffinityType =
        | ConversationTopic of ConversationTopic
        | Gaming
        | LiveShows
        | MusicGenre of Genre
        | Reading
        | Songwriting

    /// Group of traits that apply to a character.
    type Trait =
        | Awkward
        | Curious
        | Cynical
        | Disciplined
        | Dramatic
        | Empathetic
        | Gossipy
        | Guarded
        | Impulsive
        | Flirtatious
        | Networker
        | Private
        | Romantic

    /// Defines a character, be it the one that the player is controlling or any
    /// other NPC of the world.
    type Character =
        { Id: CharacterId
          Name: string
          Birthday: Date
          Gender: Gender
          Attributes: CharacterAttributes
          Moodlets: CharacterMoodlets
          Traits: Set<Trait> }

    /// Collection of skills by character.
    type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
