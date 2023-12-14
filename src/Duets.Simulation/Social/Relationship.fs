module Duets.Simulation.Social.Relationship

open Duets.Entities
open Duets.Simulation

/// Creates a `RelationshipChanged` effect that adds a new relationship between
/// the current character and the given one.
let createWith (character: Character) meetingCity state =
    let currentDate = Queries.Calendar.today state

    let relationship =
        { Character = character.Id
          Level = 0<relationshipLevel>
          MeetingCity = meetingCity
          RelationshipType = Bandmate
          LastIterationDate = currentDate }

    RelationshipChanged(character, meetingCity, Some relationship)
