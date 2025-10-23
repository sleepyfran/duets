module Duets.Simulation.Events.Band.Relationships

open Duets.Entities
open Duets.Simulation

/// Adds a relationship between the player and the given NPC as band mates after
/// they've been hired to the band.
let addWithMember (npc: Character) state =
    let currentBand = Queries.Bands.currentBand state

    Social.Relationship.createWith npc currentBand.OriginCity state
    |> List.singleton

/// Removes a relationship between the player and the given NPC after they've
/// been fired from the band.
let removeWithMember characterId state =
    let npc = Queries.Characters.find state characterId

    let currentRelationship =
        Queries.Relationship.withCharacter characterId state

    match currentRelationship with
    | Some relationship ->
        RelationshipChanged(npc, relationship.MeetingCity, None)
        |> List.singleton
    | None ->
        [] (* Shouldn't happen, but nothing to do since it's already removed. *)
