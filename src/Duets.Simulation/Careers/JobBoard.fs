module Duets.Simulation.Careers.JobBoard

open Duets.Common
open Duets.Data.Careers
open Duets.Data.World
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Careers.Common

let private placeTypeForJobType jobType =
    match jobType with
    | Barista -> PlaceTypeIndex.Cafe
    | Bartender -> PlaceTypeIndex.Bar
    | Chef -> PlaceTypeIndex.Restaurant
    | MusicProducer -> PlaceTypeIndex.Studio
    | RadioHost -> PlaceTypeIndex.RadioStudio

let private findSuitableCareerStage state (careerStages: CareerStage list) =
    careerStages
    |> List.takeWhile (fun stage ->
        fulfillsRequirements state stage.Requirements -2<adjustment>)

let private findSuitableJob state careerId cityId placeId roomId stages =
    findSuitableCareerStage state stages
    |> List.tryLast
    |> Option.map (createCareerStage cityId placeId)
    |> Option.map (fun stage ->
        { Id = careerId
          CurrentStage = stage
          Location = cityId, placeId, roomId })

let private generateBartenderJob state cityId placeId =
    findSuitableJob
        state
        Bartender
        cityId
        placeId
        Ids.Common.bar
        BartenderCareer.stages

let private generateBaristaJob state cityId placeId =
    findSuitableJob
        state
        Barista
        cityId
        placeId
        Ids.Common.cafe
        BaristaCareer.stages

let private generateMusicProducerJob state cityId placeId =
    findSuitableJob
        state
        MusicProducer
        cityId
        placeId
        Ids.Studio.masteringRoom
        MusicProducerCareer.stages

let private generateRadioHostJob state cityId placeId =
    findSuitableJob
        state
        RadioHost
        cityId
        placeId
        Ids.Studio.recordingRoom
        RadioHostCareer.stages

let private generateChefJob state cityId placeId =
    findSuitableJob
        state
        Chef
        cityId
        placeId
        Ids.Restaurant.kitchen
        ChefCareer.stages

let private generateJobsForPlace state cityId place =
    place.Rooms
    |> World.Graph.nodes
    |> List.choose (fun room ->
        match room.RoomType with
        | RoomType.Bar -> generateBartenderJob state cityId place.Id
        | RoomType.Cafe -> generateBaristaJob state cityId place.Id
        | RoomType.MasteringRoom ->
            generateMusicProducerJob state cityId place.Id
        | RoomType.RecordingRoom -> generateRadioHostJob state cityId place.Id
        | RoomType.Kitchen -> generateChefJob state cityId place.Id
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
