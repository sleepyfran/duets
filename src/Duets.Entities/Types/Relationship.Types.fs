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
    type affinity

    [<Measure>]
    type attraction

    [<Measure>]
    type familiarity

    /// Defines a relationship between the main character and an NPC.
    type Relationship =
        {
            Character: CharacterId
            MeetingCity: CityId
            LastIterationDate: Date
            RelationshipType: RelationshipType
            /// Traits about the NPC that has been already discovered.
            DiscoveredTraits: Set<Trait>
            /// Ranging between -100 and 100, defines the affinity in likes and behavior.
            Affinity: int<affinity>
            /// Ranging between -100 and 100, defines the sexual attraction.
            Attraction: int<attraction>
            /// Ranging between 0 and 100 defines the familiarity (time spent together).
            Familiarity: int<familiarity>
        }

    type RelationshipsByCharacterId = Map<CharacterId, Relationship>
    type RelationshipsByMeetingCity = Map<CityId, Set<CharacterId>>

    /// Defines all relationships for a character. A non-existent key means
    /// that the character has no relationship with that character ID.
    type Relationships =
        { ByCharacterId: RelationshipsByCharacterId
          ByMeetingCity: RelationshipsByMeetingCity }
