module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Data.Careers
open Duets.Entities
open Duets.Simulation

let private placeTypeForJobType jobType =
    match jobType with
    | Bartender -> PlaceTypeIndex.Bar
    | Barista -> PlaceTypeIndex.Cafe
    | MusicProducer -> PlaceTypeIndex.Studio

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

let private findSuitableJob state careerId cityId placeId stages =
    findSuitableCareerStage state stages
    |> List.tryLast
    |> Option.map (Common.createCareerStage cityId placeId)
    |> Option.map (fun stage ->
        { Id = careerId
          CurrentStage = stage
          Location = cityId, placeId })

let private generateBartenderJob state cityId placeId =
    findSuitableJob state Bartender cityId placeId BartenderCareer.stages

let private generateBaristaJob state cityId placeId =
    findSuitableJob state Barista cityId placeId BaristaCareer.stages

let private generateMusicProducerJob state cityId placeId =
    findSuitableJob
        state
        MusicProducer
        cityId
        placeId
        MusicProducerCareer.stages

let private generateJobsForPlace state cityId place =
    place.Rooms
    |> World.Graph.nodes
    |> List.choose (fun room ->
        match room.RoomType with
        | RoomType.Bar -> generateBartenderJob state cityId place.Id
        | RoomType.Cafe -> generateBaristaJob state cityId place.Id
        | RoomType.MasteringRoom ->
            generateMusicProducerJob state cityId place.Id
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
