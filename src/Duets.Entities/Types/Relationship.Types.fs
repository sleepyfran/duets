namespace Duets.Entities

[<AutoOpen>]
module RelationshipTypes =
    /// Defines the type of relationship between the character and an NPC.
    type RelationshipType =
        /// Unknown determines that the characters haven't met yet but the
        /// playable character has discovered traits from the other.
        | Unknown
        /// The two characters have met at some point.
        | Friend
        /// The two characters are part of the same band.
        | Bandmate

    [<Measure>]
    type relationshipLevel

    /// Defines a relationship between the main character and an NPC.
    type Relationship =
        { Character: CharacterId
          MeetingCity: CityId
          LastIterationDate: Date
          RelationshipType: RelationshipType
          DiscoveredTraits: Set<PersonalityTrait>
          Level: int<relationshipLevel> }

    type RelationshipsByCharacterId = Map<CharacterId, Relationship>
    type RelationshipsByMeetingCity = Map<CityId, Set<CharacterId>>

    /// Defines all relationships for a character. A non-existent key means
    /// that the character has no relationship with that character ID.
    type Relationships =
        { ByCharacterId: RelationshipsByCharacterId
          ByMeetingCity: RelationshipsByMeetingCity }
