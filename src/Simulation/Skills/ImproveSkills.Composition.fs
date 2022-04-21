module Simulation.Skills.ImproveSkills.Composition

open Common
open Entities
open Simulation

/// Grants a 50% chance of improving the composition, genre and instrument of
/// all members of the band between 0 and 5, generated random for each member.
let improveBandSkillsAfterComposing band state =
    Queries.Bands.currentBandMembers state
    |> List.collect (fun currentMember ->
        applySkillModificationChance
            state
            currentMember.CharacterId
            [ SkillId.Composition
              SkillId.Genre(band.Genre)
              SkillId.Instrument(currentMember.Role) ]
            50
            1)
