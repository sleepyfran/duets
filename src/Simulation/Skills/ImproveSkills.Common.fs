module Simulation.Skills.ImproveSkills.Common

open Common
open Entities
open Simulation

let private improveSkillBy ((skill, level): SkillWithLevel) amount =
    level + amount
    |> Math.clamp 0 100
    |> Tuple.two skill
    |> fun updatedSkill -> Diff((skill, level), updatedSkill)

/// Sums the given amount to the current skill level of the given character for
/// all the skills specified, keeping the level of all them between 0 and 100.
let modifyCharacterSkills state (character: Character) skills amount =
    skills
    |> List.map (Queries.Skills.characterSkillWithLevel state character.Id)
    |> List.filter (fun (_, level) -> level < 100)
    |> List.map (fun skill ->
        improveSkillBy skill amount
        |> Tuple.two character
        |> SkillImproved)

/// Generates a random number between 0 and 100, and if the chance given is
/// below or equal to that number then sums the given amount to all levels of
/// all specified skills.
let applySkillModificationChance
    state
    characterId
    skills
    chance
    improvementAmount
    =
    let random = RandomGen.genBetween 0 100

    let character =
        Queries.Characters.find state characterId

    if random > chance then
        modifyCharacterSkills state character skills improvementAmount
    else
        []
