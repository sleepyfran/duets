module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar
    | Barista -> PlaceTypeIndex.Cafe

let private generateBartenderJob cityId placeId shop =
    let shopModifier = shop.PriceModifier |> decimal

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
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect =
        [ CharacterAttribute.Energy, -20
          CharacterAttribute.Mood, -10
          CharacterAttribute.Health, -2 ] }

let private generateBaristaJob cityId placeId shop =
    let shopModifier = shop.PriceModifier |> decimal

    let initialCareerStage =
        BaristaCareer.stages
        |> List.head
        |> fun careerStage ->
            { careerStage with
                BaseSalaryPerDayMoment =
                    careerStage.BaseSalaryPerDayMoment * shopModifier }

    { Id = Barista
      CurrentStage = initialCareerStage
      Location = cityId, placeId
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ] }

let private generateJobs cityId (places: Place list) =
    places
    |> List.choose (fun place ->
        match place.Type with
        | PlaceType.Bar shop ->
            generateBartenderJob cityId place.Id shop |> Some
        | PlaceType.Cafe shop -> generateBaristaJob cityId place.Id shop |> Some
        | _ -> None)

/// Generates a list of available jobs for the given type in the current city in
/// which the player is located.
let availableJobsInCurrentCity state jobType =
    let currentCity = Queries.World.currentCity state

    placeTypeForJobType jobType
    |> Queries.World.placeIdsByTypeInCity currentCity.Id
    |> List.map (Queries.World.placeInCurrentCityById state)
    |> List.chunkBySize 5
    |> List.trySample
    |> Option.map (generateJobs currentCity.Id)
    |> Option.defaultValue []
