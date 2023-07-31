module Duets.Simulation.Skills.Improve.Composition

open Duets.Entities
open Duets.Simulation

/// Grants a 50% chance of improving the composition, genre and instrument of
/// all members of the band between 0 and 5, generated random for each member.
let improveBandSkillsChance band state =
    Queries.Bands.currentBandMembers state
    |> List.collect (fun currentMember ->
        Common.applySkillModificationChance
            state
            {| CharacterId = currentMember.CharacterId
               Skills =
                [ SkillId.Composition
                  SkillId.Genre(band.Genre)
                  SkillId.Instrument(currentMember.Role) ]
               Chance = 50
               ImprovementAmount = 1 |})
