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

    /// Personality traits that control the temperament of the character.
    type TemperamentPersonalityTrait =
        | Awkward
        | Curious
        | Cynical
        | Disciplined
        | Dramatic
        | Impulsive
        | Guarded
        | LaidBack
        | Proud
        | Warm

    /// Lifestyle traits that control how the character acts overall and their hobbies.
    type LifeStylePersonalityTrait =
        | FitnessRegular
        | Foodie
        | Homebody
        | PartyRegular
        | Traveler
        | Workaholic

    /// Personality traits that control the music affinity of the character.
    type MusicPersonalityTrait =
        | GearNerd
        | GenrePurist of Genre
        | MusicFan of Genre
        | SceneVeteran

    /// Personality traits that control the way the character behaves socially.
    type SocialPersonalityTrait =
        | Empathetic
        | Gossipy
        | Flirtatious
        | Networker
        | Private
        | Romantic

    /// Group of traits that apply to a character.
    type PersonalityTrait =
        | LifestyleTrait of LifeStylePersonalityTrait
        | MusicTrait of MusicPersonalityTrait
        | SocialTrait of SocialPersonalityTrait
        | TemperamentTrait of TemperamentPersonalityTrait

    /// Defines a character, be it the one that the player is controlling or any
    /// other NPC of the world.
    type Character =
        { Id: CharacterId
          Name: string
          Birthday: Date
          Gender: Gender
          Attributes: CharacterAttributes
          Moodlets: CharacterMoodlets
          Traits: Set<PersonalityTrait> }

    /// Collection of skills by character.
    type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
