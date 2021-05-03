module Simulation.Bands.Members

open Common
open Entities
open Simulation.Character.Queries
open Simulation.Calendar.Queries
open Storage

/// Derives an age that will be +-5 of a given average but no less than 18
/// or more than 80.
let private ageFromAverage avg =
  System.Random().Next(-5, 5)
  |> (+) avg
  |> Math.clamp 18 80

/// Creates a member that is available for being hired. The member will have
/// some skills auto-generated around the given `averageSkillLevel` for the
/// specified genre, instrument and plus the composition skill and also an
/// age around the average age of the band.
let private createMemberForHire averageSkillLevel averageAge genre instrument =
  [ Composition
    Instrument instrument
    Genre genre ]
  |> List.map (fun id -> Skill.createFromAverageLevel id averageSkillLevel)
  |> fun skills ->
       let npc =
         Database.randomNpc ()
         |> fun (name, gender) ->
              Character.from name (ageFromAverage averageAge) gender
              |> Result.unwrap

       Band.MemberForHire.from npc instrument.Type skills

/// Generates an infinite sequence of available members for the given band
/// looking for the given instrument.
let membersForHire band instrument =
  let averageSkillLevel =
    Queries.averageSkillLevel band
    |> Math.roundToNearest

  let averageAge =
    Queries.averageAge band |> Math.roundToNearest

  Seq.initInfinite
    (fun _ ->
      createMemberForHire averageSkillLevel averageAge band.Genre instrument)

/// Processes the given member for hire into a current member of the band.
let hireMember band (memberForHire: MemberForHire) =
  let currentMember =
    today ()
    |> Band.Member.fromMemberForHire memberForHire

  State.modifyState
    (fun state ->
      { state with
          Bands =
            band.Members
            |> List.append [ currentMember ]
            |> fun updatedMembers ->
                 Map.add
                   band.Id
                   { band with Members = updatedMembers }
                   state.Bands })

type FireError = AttemptToFirePlayableCharacter

/// Removes a current member from the band and adds it to the past members with
/// today as the date it was fired.
let fireMember band (bandMember: CurrentMember) =
  let character = playableCharacter ()

  if bandMember.Character.Id = character.Id then
    Error AttemptToFirePlayableCharacter
  else
    let pastMember =
      Band.PastMember.fromMember bandMember (Date <| today ())

    State.modifyState
      (fun state ->
        { state with
            Bands =
              band.Members
              |> List.filter
                   (fun m -> m.Character.Id <> bandMember.Character.Id)
              |> fun updatedMembers ->
                   Map.add
                     band.Id
                     { band with
                         Members = updatedMembers
                         PastMembers =
                           List.append [ pastMember ] band.PastMembers }
                     state.Bands })

    Ok()
