module Simulation.Careers.JobBoard

open Common
open Data.Careers
open Entities
open Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar

let private generateBartenderJob cityId placeId shop =
    let shopModifier =
        shop.PriceModifier |> decimal

    let initialCareerStage =
        BartenderCareer.stages
        |> List.head
        |> fun careerStage ->
            { careerStage with
                BaseSalaryPerDayMoment =
                    careerStage.BaseSalaryPerDayMoment * shopModifier }

    { Id = Bartender
      CurrentStage = initialCareerStage
      Location = cityId, placeId
      Schedule = JobSchedule.Free }

let private generateJobs cityId (places: Place list) =
    places
    |> List.choose (fun place ->
        match place.Type with
        | PlaceType.Bar shop ->
            generateBartenderJob cityId place.Id shop |> Some
        | _ -> None)

/// Generates a list of available jobs for the given type in the current city in
/// which the player is located.
let availableJobsInCurrentCity state jobType =
    let currentCity =
        Queries.World.currentCity state

    placeTypeForJobType jobType
    |> Queries.World.placeIdsOf currentCity.Id
    |> List.map (Queries.World.placeInCurrentCityById state)
    |> List.chunkBySize 5
    |> List.sample
    |> generateJobs currentCity.Id
