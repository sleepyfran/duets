module Duets.Simulation.State.Skills

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let add (characterId: CharacterId) (skillWithLevel: SkillWithLevel) =
    let (skill, _) = skillWithLevel

    let skillLens =
        Lenses.State.characterSkills_
        >-> Map.keyWithDefault_ characterId Map.empty

    let addSkill map = Map.add skill.Id skillWithLevel map

    Optic.map skillLens addSkill

let addMultiple
    (characterId: CharacterId)
    (skillsWithLevel: SkillWithLevel list)
    state
    =
    skillsWithLevel
    |> List.fold (fun acc skill -> add characterId skill acc) state
