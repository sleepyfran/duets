module Simulation.State.Skills

open Aether
open Aether.Operators
open Common
open Entities

let add (characterId: CharacterId) (skillWithLevel: SkillWithLevel) =
    let (skill, _) = skillWithLevel

    let skillLens =
        Lenses.State.characterSkills_
        >-> Map.keyWithDefault_ characterId Map.empty

    let addSkill map = Map.add skill.Id skillWithLevel map

    Optic.map skillLens addSkill
