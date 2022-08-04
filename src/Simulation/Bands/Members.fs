module Simulation.Bands.Members

open Aether
open Common
open Entities
open Simulation

/// Derives an age that will be +-5 of a given average but no less than 18
/// or more than 80.
let private birthdayFromAverageAge state avg =
    let computedAge =
        System.Random().Next(-5, 5)
        |> (+) avg
        |> Math.clamp 18 80

    let today = Queries.Calendar.today state
    today |> Calendar.Ops.addYears -computedAge

/// Creates a member that is available for being hired. The member will have
/// some skills auto-generated around the given `averageSkillLevel` for the
/// specified genre, instrument and plus the composition skill and also an
/// age around the average age of the band.
let private createMemberForHire
    state
    averageSkillLevel
    averageAge
    genre
    instrument
    =
    [ SkillId.Composition
      SkillId.Instrument instrument
      SkillId.Genre genre ]
    |> List.map (fun id -> Skill.createFromAverageLevel id averageSkillLevel)
    |> fun skills ->
        let npc =
            Data.Npcs.random ()
            |> fun (name, gender) ->
                Character.from
                    name
                    gender
                    (birthdayFromAverageAge state averageAge)

        Band.MemberForHire.from npc instrument skills

/// Generates an infinite sequence of available members for the given band
/// looking for the given instrument.
let membersForHire state band instrument =
    let averageSkillLevel =
        Queries.Bands.averageSkillLevel state band
        |> Math.roundToNearest

    let averageAge =
        Queries.Bands.averageAge state band
        |> Math.roundToNearest

    Seq.initInfinite (fun _ ->
        createMemberForHire
            state
            averageSkillLevel
            averageAge
            band.Genre
            instrument)

/// Processes the given member for hire into a current member of the band.
let hireMember state (band: Band) (memberForHire: MemberForHire) =
    Queries.Calendar.today state
    |> Band.Member.fromMemberForHire memberForHire
    |> fun currentMember ->
        MemberHired(
            band,
            memberForHire.Character,
            currentMember,
            memberForHire.Skills
        )

type FireError = AttemptToFirePlayableCharacter

/// Removes a current member from the band and adds it to the past members with
/// today as the date it was fired.
let fireMember state (band: Band) (bandMember: CurrentMember) =
    let character =
        Queries.Characters.playableCharacter state

    if bandMember.CharacterId = character.Id then
        Error AttemptToFirePlayableCharacter
    else
        let pastMember =
            Band.PastMember.fromMember bandMember (Queries.Calendar.today state)

        (band, bandMember, pastMember)
        |> MemberFired
        |> Ok
