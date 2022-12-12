module Data.Careers

open Entities

[<RequireQualifiedAccess>]
module Careers =
    let all = [ Bartender ]

[<RequireQualifiedAccess>]
module BartenderCareer =
    let stages =
        [ { Id = CareerStageId 0uy
            BaseSalaryPerDayMoment = 2.5m<dd> } ]
