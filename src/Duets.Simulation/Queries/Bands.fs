namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

module Bands =
    /// Returns the band given its ID.
    let ofCharacterById state bandId =
        let lens = Lenses.State.bands_ >-> Lenses.Bands.characterBand_ bandId
        Optic.get lens state |> Option.get

    /// Returns the ID of the currently selected character band in the game.
    let currentBandId =
        Lenses.State.bands_ >-> Lenses.Bands.current_ |> Optic.get

    /// Returns a band that matches the given ID.
    let byId state bandId =
        let characterBands =
            Lenses.State.bands_ >-> Lenses.Bands.characterBand_ bandId

        let simulatedBands =
            Lenses.State.bands_ >-> Lenses.Bands.simulatedBand_ bandId

        Optic.get characterBands state
        |> Option.orElse (Optic.get simulatedBands state)
        |> Option.get

    /// Returns the currently selected character band in the game.
    let currentBand state = currentBandId state |> byId state

    /// Returns all current members of the current band.
    let currentBandMembers state =
        currentBand state |> Optic.get Lenses.Band.members_

    /// Returns all past members of the current band.
    let pastBandMembers state =
        currentBand state |> Optic.get Lenses.Band.pastMembers_

    /// Returns a member that matches the given characterId
    let currentMemberById state characterId =
        currentBandMembers state
        |> List.tryFind (fun cm -> cm.CharacterId = characterId)

    /// Returns all the current members of the current band removing the main
    /// character out of it. Useful for situations like selections in firing or
    /// actions in which the playable character should not be taken into
    /// consideration.
    let currentBandMembersWithoutPlayableCharacter state =
        let mainCharacter = Characters.playableCharacter state

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
        |> List.map (_.CharacterId)
        |> List.map (Skills.averageSkillLevel state)
        |> List.average

    /// Returns the average age of the members of the band.
    let averageAge state (band: Band) =
        band.Members
        |> List.map (fun currentMember ->
            Characters.find state currentMember.CharacterId)
        |> List.averageBy (fun character ->
            Characters.ageOf state character |> float)

    /// Returns the sum of all fans in the given fan base.
    let totalFans fanBase = fanBase |> List.ofMapValues |> List.sum

    /// Returns the total amount of fans of the band across all cities.
    let totalFans' band = band.Fans |> totalFans

    /// Returns the total amount of fans of the band in the given city.
    let fansInCity (fanBase: FanBaseByCity) cityId =
        let lens = Map.key_ cityId
        Optic.get lens fanBase |> Option.defaultValue 0<fans>

    /// Returns the total amount of fans of the band in the given city.
    let fansInCity' band cityId = fansInCity band.Fans cityId

    /// Gives an estimate of the band's fame between 0 and 100 based on the
    /// total amount of people willing to listen to the band's genre.
    let estimatedFameLevel state (bandId: BandId) =
        let band = byId state bandId

        let normalizedMarketSize =
            Genres.usefulMarketOf state band.Genre |> System.Math.Log10

        let totalFans = totalFans' band
        let normalizedFans = System.Math.Log10(totalFans |> double)

        let fameScalingFactor =
            match totalFans with
            | fans when fans < 500<fans> -> 8.0
            | fans when fans < 1000<fans> -> 4.0
            | fans when fans < 10000<fans> -> 2.5
            | fans when fans < 100000<fans> -> 2.0
            | fans when fans < 800000<fans> -> 1.5
            | _ -> 1

        (normalizedFans / (fameScalingFactor * normalizedMarketSize)) * 100.0
        |> Math.roundToNearest
        |> Math.clamp 1 100

    /// Returns all simulated bands.
    let allSimulated state =
        let simulatedLens_ =
            Lenses.State.bands_ >-> Lenses.Bands.simulatedBands_

        Optic.get simulatedLens_ state

    /// Returns whether the given band has any member with the given role.
    let hasAnyMemberWithRole (band: Band) (role: InstrumentType) =
        band.Members |> List.exists (fun bandMember -> bandMember.Role = role)

    /// Returns whether the current band has any member with the given role.
    let currentBandHasAnyMemberWithRole state role =
        let band = currentBand state
        hasAnyMemberWithRole band role
