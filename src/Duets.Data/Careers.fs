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
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 1) ] }
          // Junior Barista
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 10m<dd>
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 10) ] }
          // Barista
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 12m<dd>
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 40) ] }
          // Senior Barista
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 15m<dd>
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 60) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 20m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] } ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ // Dishwasher
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7.5m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 1) ] }
          // Table cleaner
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 11m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 10) ] }
          // Bartender
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 15m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 40) ] }
          // Mixologist
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 18m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 60) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 25m<dd>
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] } ]
