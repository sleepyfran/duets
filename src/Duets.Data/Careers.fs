module Duets.Data.Careers

open Duets.Entities

[<RequireQualifiedAccess>]
module Careers =
    let all = [ Barista; Bartender ]

[<RequireQualifiedAccess>]
module BaristaCareer =
    let stages =
        [ // Dishwasher
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 10) ] }
          // Junior Barista
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 10m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 40) ] }
          // Barista
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 12m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 60) ] }
          // Senior Barista
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 15m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 20m<dd>
            NextStageRequirements = [] } ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ // Dishwasher
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7.5m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 10) ] }
          // Table cleaner
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 11m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 40) ] }
          // Bartender
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 15m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 60) ] }
          // Mixologist
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 18m<dd>
            NextStageRequirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 25m<dd>
            NextStageRequirements = [] } ]
