[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.CityDependentDescriptions

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Cli.Text.World

let private noDescription _ _ = [ "" ]

let private streetDescriptionById cityId streetId =
    let street = Queries.World.streetById cityId streetId

    match cityId with
    | Prague -> Prague.ofStreet street
    | _ -> noDescription

let cityDependentDescription (place: Place) (_: RoomType) : string =
    let state = State.get ()
    let city = Queries.World.currentCity state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    let currentDayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    let generator =
        match place.PlaceType with
        | Street -> streetDescriptionById city.Id place.Id
        | _ -> noDescription

    generator zone currentDayMoment |> List.sample
