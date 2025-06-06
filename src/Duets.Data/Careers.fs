module Duets.Data.Careers

open Duets.Common
open Duets.Entities

[<RequireQualifiedAccess>]
module Careers =
    let all: CareerId list = Union.allCasesOf<CareerId> ()

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

[<RequireQualifiedAccess>]
module ChefCareer =
    let stages: CareerStage list =
        [ // Kitchen Porter
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 12m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -15 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Cooking, 5) ] }
          // Commis Chef
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 18m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -12 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Cooking, 20) ] }
          // Chef de Partie
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 28m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -10; CharacterAttribute.Mood, 2 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Cooking, 45) ] }
          // Sous Chef
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 45m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -10; CharacterAttribute.Mood, 4 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Cooking, 70) ] }
          // Head Chef (Executive Chef)
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 70m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -8; CharacterAttribute.Mood, 6 ]
            Requirements = [ CareerStageRequirement.Skill(SkillId.Cooking, 90) ] } ]

[<RequireQualifiedAccess>]
module MusicProducerCareer =
    let stages =
        [ // Assistant Producer
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 10.5m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.MusicProduction, 20) ] }
          // Junior Producer
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 15m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.MusicProduction, 40) ] }
          // Producer
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 30m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.MusicProduction, 60) ] }
          // Senior Producer
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 45m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -9 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.MusicProduction, 80) ] }
          // Distinguished Producer
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 60m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -8 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.MusicProduction, 100)
                CareerStageRequirement.Skill(SkillId.Speech, 60) ] } ]

[<RequireQualifiedAccess>]
module RadioHostCareer =
    let stages =
        [ // Station Intern
          { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 8m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Presenting, 1) ] }
          // Junior On-Air Talent
          { Id = CareerStageId 1uy
            BaseSalaryPerDayMoment = 15m<dd>
            Schedule = JobSchedule.Free 2<dayMoments>
            ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Presenting, 20)
                CareerStageRequirement.Fame 10 ] }
          // Radio Host
          { Id = CareerStageId 2uy
            BaseSalaryPerDayMoment = 30m<dd>
            Schedule = JobSchedule.Free 3<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -12; CharacterAttribute.Mood, 2 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Presenting, 40)
                CareerStageRequirement.Skill(SkillId.Speech, 20)
                CareerStageRequirement.Fame 25 ] }
          // Lead Program Host
          { Id = CareerStageId 3uy
            BaseSalaryPerDayMoment = 60m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -10; CharacterAttribute.Mood, 5 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Presenting, 70)
                CareerStageRequirement.Skill(SkillId.Speech, 50)
                CareerStageRequirement.Fame 50 ] }
          // Network Star Presenter
          { Id = CareerStageId 4uy
            BaseSalaryPerDayMoment = 90m<dd>
            Schedule = JobSchedule.Free 4<dayMoments>
            ShiftAttributeEffect =
              [ CharacterAttribute.Energy, -8; CharacterAttribute.Mood, 8 ]
            Requirements =
              [ CareerStageRequirement.Skill(SkillId.Presenting, 100)
                CareerStageRequirement.Skill(SkillId.Speech, 70)
                CareerStageRequirement.Fame 75 ] } ]
