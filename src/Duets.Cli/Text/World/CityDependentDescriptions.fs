[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.CityDependentDescriptions

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private placeDescriptionByCity cityId placeType =
    match cityId, placeType with
    | LosAngeles, PlaceTypeIndex.MetroStation ->
        LosAngeles.MetroStations.description
    | LosAngeles, PlaceTypeIndex.Street -> LosAngeles.Streets.description
    | NewYork, PlaceTypeIndex.MetroStation -> NewYork.MetroStations.description
    | NewYork, PlaceTypeIndex.Street -> NewYork.Streets.description
    | Prague, PlaceTypeIndex.MetroStation -> Prague.MetroStations.description
    | Prague, PlaceTypeIndex.Street -> Prague.Streets.description
    | _ -> fun _ _ -> [ "" ]

let cityDependentDescription (place: Place) (_: RoomType) : string =
    let state = State.get ()
    let city = Queries.World.currentCity state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    let currentDayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    let generateDescriptions =
        placeDescriptionByCity
            city.Id
            (place.PlaceType |> World.Place.Type.toIndex)

    let randomDescriptor = zone.Descriptors |> List.sample
    generateDescriptions currentDayMoment randomDescriptor |> List.sample
