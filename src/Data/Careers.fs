module Data.Careers

open Entities

[<RequireQualifiedAccess>]
module Careers =
    let all = [ Barista; Bartender ]

[<RequireQualifiedAccess>]
module BaristaCareer =
    let stages =
        [ { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 2m<dd> } ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 2.5m<dd> } ]
