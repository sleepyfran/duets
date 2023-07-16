module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar
    | Barista -> PlaceTypeIndex.Cafe

let private qualitySalaryModifier cityId placeId =
    let place = Queries.World.placeInCityById cityId placeId

    match place.Quality with
    | quality when quality < 20<quality> -> 0.5m
    | quality when quality < 40<quality> -> 0.75m
    | quality when quality < 60<quality> -> 1.0m
    | quality when quality < 80<quality> -> 1.5m
    | _ -> 2m

let private generateBartenderJob cityId placeId =
    let salaryModifier = qualitySalaryModifier cityId placeId

    let initialCareerStage =
        BartenderCareer.stages
        |> List.head
        |> fun careerStage ->
            { careerStage with
                BaseSalaryPerDayMoment =
                    careerStage.BaseSalaryPerDayMoment * salaryModifier }

    { Id = Bartender
      CurrentStage = initialCareerStage
      Location = cityId, placeId
      Schedule = JobSchedule.Free 2<dayMoments>
      ShiftAttributeEffect =
        [ CharacterAttribute.Energy, -20
          CharacterAttribute.Mood, -10
          CharacterAttribute.Health, -2 ] }

let private generateBaristaJob cityId placeId =
    let salaryModifier = qualitySalaryModifier cityId placeId

    let initialCareerStage =
        BaristaCareer.stages
        |> List.head
        |> fun careerStage ->
            { careerStage with
                BaseSalaryPerDayMoment =
                    careerStage.BaseSalaryPerDayMoment * salaryModifier }

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
