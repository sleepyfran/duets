namespace Duets.Entities

[<AutoOpen>]
module RelationshipTypes =
    /// Defines the type of relationship between the character and an NPC.
    type RelationshipType =
        | Acquaintance
        | Friend
        | Bandmate

    [<Measure>]
    type relationshipLevel

    /// Defines a relationship between the main character and an NPC.
    type Relationship =
        { Character: CharacterId
          MeetingPlace: PlaceCoordinates
          RelationshipType: RelationshipType
          Level: int<relationshipLevel> }

    /// Defines all relationships for a character. A non-existent key means
    /// that the character has no relationship with that character ID.
    type Relationships = Map<CharacterId, Relationship>
