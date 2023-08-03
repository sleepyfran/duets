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
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 1) ] }
          // Junior Barista
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 10m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 10) ] }
          // Barista
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 12m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 40) ] }
          // Senior Barista
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 15m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -8 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Barista, 60) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 20m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -7 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Barista, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] } ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ // Dishwasher
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7.5m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -20
                CharacterAttribute.Mood, -10
                CharacterAttribute.Health, -2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 1) ] }
          // Table cleaner
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 11m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -18
                CharacterAttribute.Mood, -8
                CharacterAttribute.Health, -2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 10) ] }
          // Bartender
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 15m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -15
                CharacterAttribute.Mood, -5
                CharacterAttribute.Health, -2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 40) ] }
          // Mixologist
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 18m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -15; CharacterAttribute.Health, -2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 60) ] }
          // Manager
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 25m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -10; CharacterAttribute.Health, -2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Bartending, 80)
                CareerStageRequirement.Skill(SkillId.Speech, 50) ] } ]
