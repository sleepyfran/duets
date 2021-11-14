module Simulation.Skills.ImproveSkills

open Common
open Entities
open Simulation.Queries

let improveSkillByOne ((skill, level): SkillWithLevel) =
    level + 1
    |> Tuple.two skill
    |> fun updatedSkill -> Diff((skill, level), updatedSkill)

let improveMemberSkills state (band: Band) (currentMember: CurrentMember) =
    [ Composition
      Genre(band.Genre)
      SkillId.Instrument(currentMember.Role) ]
    |> List.map (
        Skills.characterSkillWithLevel state currentMember.Character.Id
    )
    |> List.filter (fun (_, level) -> level < 100)
    |> List.map (
        improveSkillByOne
        >> Tuple.two currentMember.Character
    )

let improveBandMembersSkills state band =
    Bands.currentBandMembers state
    |> List.map (improveMemberSkills state band)
    |> List.concat
    |> List.map SkillImproved

let improveBandSkillsAfterComposing' randomBetween state band =
    if randomBetween 0 100 > 50 then
        improveBandMembersSkills state band
    else
        []

/// Grants a 50% chance of improving the composition, genre and instrument of
/// all members of the band between 0 and 5, generated random for each member.
let improveBandSkillsAfterComposing =
    improveBandSkillsAfterComposing' Random.between
