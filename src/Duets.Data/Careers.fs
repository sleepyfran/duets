module Duets.Data.Careers

open Duets.Entities

[<RequireQualifiedAccess>]
module Careers =
    let all = [ Barista; Bartender ]

[<RequireQualifiedAccess>]
module BaristaCareer =
    let stages =
        [ { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7.5m<dd> } ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 7.9m<dd> } ]
