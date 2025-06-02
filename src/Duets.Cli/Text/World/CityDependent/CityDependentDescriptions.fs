[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.CityDependentDescriptions

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private placeDescriptionByCity =
    [ LosAngeles,
      [ PlaceTypeIndex.MetroStation, LosAngeles.MetroStations.description
        PlaceTypeIndex.Street, LosAngeles.Streets.description ]
      |> Map.ofList ]
    |> Map.ofList

let cityDependentDescription (place: Place) (_: RoomType) : string =
    let state = State.get ()
    let city = Queries.World.currentCity state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    let currentDayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    let generateDescriptions =
        placeDescriptionByCity
        |> Map.find city.Id
        |> Map.find (place.PlaceType |> World.Place.Type.toIndex)

    let randomDescriptor = zone.Descriptors |> List.sample
    generateDescriptions currentDayMoment randomDescriptor |> List.sample
