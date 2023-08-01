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

/// Creates a career stage with salary adjusted for the city and place quality.
let createCareerStage cityId placeId careerStage =
    let salaryModifier = salaryModifiers cityId placeId

    { careerStage with
        BaseSalaryPerDayMoment =
            careerStage.BaseSalaryPerDayMoment * salaryModifier }
