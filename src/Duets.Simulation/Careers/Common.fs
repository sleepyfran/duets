module Duets.Simulation.Careers.Common

open Duets.Common.Operators
open Duets.Entities
open Duets.Simulation

let private salaryModifiers cityId placeId =
    let city = Queries.World.cityById cityId
    let costOfLivingModifier = decimal city.CostOfLiving
    let place = Queries.World.placeInCityById cityId placeId

    let qualityModifier =
        match place.Quality with
        | quality when quality < 60<quality> -> 0.5m
        | quality when quality < 80<quality> -> 0.7m
        | quality when quality >< (80<quality>, 85<quality>) -> 0.8m
        | quality when quality >< (85<quality>, 90<quality>) -> 0.9m
        | quality when quality >< (90<quality>, 95<quality>) -> 1.0m
        | _ -> 1.1m

    costOfLivingModifier * qualityModifier

[<Measure>]
type adjustment

/// Checks wether the playable character fulfills the given requirements for
/// a certain career stage. Accepts a `requirementAdjustment` parameter that will
/// be subtracted from the requirement level where applicable so that it's lower
/// or raised. For example, giving an adjustment of -2 means that skill and fame
/// levels will be reduced by 2 when comparing against the character's.
let fulfillsRequirements
    state
    requirements
    (requirementAdjustment: int<adjustment>)
    =
    let character = Queries.Characters.playableCharacter state
    let adjustmentInt = requirementAdjustment / 1<adjustment>

    requirements
    |> List.forall (function
        | CareerStageRequirement.Skill(skillId, minLevel) ->
            let _, currentSkillLevel =
                Queries.Skills.characterSkillWithLevel
                    state
                    character.Id
                    skillId

            currentSkillLevel >= minLevel + adjustmentInt
        | CareerStageRequirement.Fame(fameLevel) ->
            let characterFame =
                Queries.Characters.playableCharacterAttribute
                    state
                    CharacterAttribute.Fame

            characterFame >= fameLevel + adjustmentInt)

/// Creates a career stage with salary adjusted for the city and place quality.
let createCareerStage cityId placeId careerStage =
    let salaryModifier = salaryModifiers cityId placeId

    { careerStage with
        BaseSalaryPerDayMoment =
            careerStage.BaseSalaryPerDayMoment * salaryModifier }
