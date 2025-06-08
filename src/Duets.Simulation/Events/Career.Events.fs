module Duets.Simulation.Events.Career

open Duets.Entities
open Duets.Simulation

/// Runs all the events associated with effects of a career. For example,
/// finishing a career shift will improve the character's skills and also
/// give a chance of getting promoted if the character has the required skills
/// for the next level.
let internal run effect =
    match effect with
    | CareerShiftPerformed(job, _, _) ->
        [ Careers.RequirementCharacterUpgrade.applyRequirementUpgradeChange job
          Careers.Promotion.promoteIfNeeded job ]
        |> ContinueChain
        |> Some
    | _ -> None
