namespace Duets.Entities

[<AutoOpen>]
module RelationshipTypes =
    /// Defines the type of relationship between the character and an NPC.
    type RelationshipType =
        | Friend
        | Bandmate

    [<Measure>]
    type relationshipLevel

    /// Defines a relationship between the main character and an NPC.
    type Relationship =
        { Character: CharacterId
          MeetingCity: CityId
          LastIterationDate: Date
          RelationshipType: RelationshipType
          Level: int<relationshipLevel> }

    type RelationshipsByCharacterId = Map<CharacterId, Relationship>
    type RelationshipsByMeetingCity = Map<CityId, Set<CharacterId>>

    /// Defines all relationships for a character. A non-existent key means
    /// that the character has no relationship with that character ID.
    type Relationships =
        { ByCharacterId: RelationshipsByCharacterId
          ByMeetingCity: RelationshipsByMeetingCity }
