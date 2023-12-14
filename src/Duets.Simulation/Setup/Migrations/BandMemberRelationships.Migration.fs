module rec Duets.Simulation.Migrations.BandMemberRelationships

open Duets.Simulation
open Duets.Simulation.Social

/// Checks if it's necessary to apply a migration to the state so that all
/// band members have a relationship with the character.
let migrateIfNeeded state =
    let band = Queries.Bands.currentBand state

    let bandMembers =
        Queries.Bands.currentBandMembersWithoutPlayableCharacter state

    bandMembers
    |> List.filter (fun bandMember ->
        Queries.Relationship.withCharacter bandMember.CharacterId state
        |> Option.isNone)
    |> List.fold
        (fun updatedState bandMember ->
            migrateBandMember band bandMember updatedState)
        state

let private migrateBandMember band bandMember state =
    let npc = Queries.Characters.find state bandMember.CharacterId

    Relationship.createWith npc band.OriginCity state
    |> State.Root.applyEffect state
