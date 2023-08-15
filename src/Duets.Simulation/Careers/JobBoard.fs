module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar
    | Barista -> PlaceTypeIndex.Cafe

let private findSuitableCareerStage state (careerStages: CareerStage list) =
    let character = Queries.Characters.playableCharacter state

    careerStages
    |> List.takeWhile (fun stage ->
        stage.Requirements
        |> List.forall (function
            | CareerStageRequirement.Skill(skillId, minLevel) ->
                let _, currentSkillLevel =
                    Queries.Skills.characterSkillWithLevel
                        state
                        character.Id
                        skillId

                currentSkillLevel >= minLevel - 2))

let private findSuitableCareerStageOrDefault state cityId placeId stages =
    findSuitableCareerStage state stages
    |> List.tryLast
    |> Option.defaultValue stages.Head
    |> Common.createCareerStage cityId placeId

let private generateBartenderJob state cityId placeId =
    let careerStage =
        findSuitableCareerStageOrDefault
            state
            cityId
            placeId
            BartenderCareer.stages

    { Id = Bartender
      CurrentStage = careerStage
      Location = cityId, placeId }

let private generateBaristaJob state cityId placeId =
    let initialCareerStage =
        findSuitableCareerStageOrDefault
            state
            cityId
            placeId
            BaristaCareer.stages

    { Id = Barista
      CurrentStage = initialCareerStage
      Location = cityId, placeId }

let private generateJobsForPlace state cityId place =
    place.Rooms
    |> World.Graph.nodes
    |> List.choose (fun room ->
        match room.RoomType with
        | RoomType.Bar -> generateBartenderJob state cityId place.Id |> Some
        | RoomType.Cafe -> generateBaristaJob state cityId place.Id |> Some
        | _ -> None)

let private generateJobs state cityId (places: Place list) =
    places
    |> List.choose (fun place ->
        let jobsInPlace = generateJobsForPlace state cityId place

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
    |> Option.map (generateJobs state currentCity.Id)
    |> Option.defaultValue []
