module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Common.Operators
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar
    | Barista -> PlaceTypeIndex.Cafe

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

let private createCareerStage cityId placeId careerStage =
    let salaryModifier = salaryModifiers cityId placeId

    { careerStage with
        BaseSalaryPerDayMoment =
            careerStage.BaseSalaryPerDayMoment * salaryModifier }

let private generateBartenderJob cityId placeId =
    let initialCareerStage =
        BartenderCareer.stages |> List.head |> createCareerStage cityId placeId

    { Id = Bartender
      CurrentStage = initialCareerStage
      Location = cityId, placeId
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect =
        [ CharacterAttribute.Energy, -20
          CharacterAttribute.Mood, -10
          CharacterAttribute.Health, -2 ] }

let private generateBaristaJob cityId placeId =
    let initialCareerStage =
        BaristaCareer.stages |> List.head |> createCareerStage cityId placeId

    { Id = Barista
      CurrentStage = initialCareerStage
      Location = cityId, placeId
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect = [ CharacterAttribute.Energy, -10 ] }

let private generateJobsForPlace cityId place =
    place.Rooms
    |> World.Graph.nodes
    |> List.choose (function
        | RoomType.Bar -> generateBartenderJob cityId place.Id |> Some
        | RoomType.Cafe -> generateBaristaJob cityId place.Id |> Some
        | _ -> None)

let private generateJobs cityId (places: Place list) =
    places
    |> List.choose (fun place ->
        let jobsInPlace = generateJobsForPlace cityId place

        match jobsInPlace with
        | [] -> None
        | _ -> Some jobsInPlace)
    |> List.flatten

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
