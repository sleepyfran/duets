namespace Simulation.Queries

module Bands =
    open Aether
    open Common
    open Entities

    /// Returns the band given its ID.
    let byId state bandId = state.Bands |> Map.find bandId

    /// Returns the currently selected band in the game.
    let currentBand state =
        state.Bands |> Map.find state.CurrentBandId

    /// Returns all current members of the current band.
    let currentBandMembers state =
        currentBand state
        |> Optic.get Lenses.Band.members_

    /// Returns all past members of the current band.
    let pastBandMembers state =
        currentBand state
        |> Optic.get Lenses.Band.pastMembers_

    /// Returns a member that matches the given characterId
    let currentMemberById state characterId =
        currentBandMembers state
        |> List.tryFind (fun cm -> cm.CharacterId = characterId)

    /// Returns all the current members of the current band removing the main
    /// character out of it. Useful for situations like selections in firing or
    /// actions in which the playable character should not be taken into
    /// consideration.
    let currentBandMembersWithoutPlayableCharacter state =
        let mainCharacter =
            Characters.playableCharacter state

        currentBandMembers state
        |> List.filter (fun c -> c.CharacterId <> mainCharacter.Id)

    /// Returns the playable character as a member of the band they're currently
    /// in.
    let currentPlayableMember state =
        currentBandMembers state
        |> List.find (fun bandMember ->
            bandMember.CharacterId = state.PlayableCharacterId)

    /// Returns the average skill level of the members of the band.
    let averageSkillLevel state (band: Band) =
        band.Members
        |> List.map (fun mem -> mem.CharacterId)
        |> List.map (Skills.averageSkillLevel state)
        |> List.average

    /// Returns the average age of the members of the band.
    let averageAge state (band: Band) =
        band.Members
        |> List.map (fun currentMember ->
            Characters.find state currentMember.CharacterId)
        |> List.averageBy (fun character -> float character.Age)
