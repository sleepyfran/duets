module Simulation.Bands.Members

open Common
open Entities
open Lenses
open Simulation.Bands.Queries
open Simulation.Calendar.Queries
open Storage

/// Derives an age that will be +-5 of a given average but no less than 18
/// or more than 80.
let ageFromAverage avg =
  System.Random().Next(-5, 5)
  |> (+) avg
  |> Math.clamp 18 80

/// Creates a member that is available for being hired. The member will have
/// some skills auto-generated around the given `averageSkillLevel` for the
/// specified genre, instrument and plus the composition skill and also an
/// age around the average age of the band.
let createMemberForHire averageSkillLevel averageAge genre instrument =
  [ Composition
    Instrument instrument
    Genre genre ]
  |> List.map (fun id -> Skill.createFromAverageLevel id averageSkillLevel)
  |> fun skills ->
       { Character =
           Database.randomNpc ()
           |> fun (name, gender) ->
                Character.from name (ageFromAverage averageAge) gender
                |> Result.unwrap

         Role = instrument.Type
         Skills = skills }

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
let hireMember (memberForHire: MemberForHire) =
  let band = currentBand ()

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
