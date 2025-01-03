[<RequireQualifiedAccess>]
module Duets.Cli.Text.World.Streets

open Duets.Agents
open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns a description for the given street that matches the street's zone's
/// descriptors, city and current day moment.
let description (place: Place) (_: RoomType) =
    let state = State.get ()
    let city = Queries.World.currentCity state
    let zone = Queries.World.zoneInCurrentCityById state place.ZoneId

    let currentDayMoment =
        Queries.Calendar.today state |> Calendar.Query.dayMomentOf

    let cityGenerator =
        match city.Id with
        | LosAngeles -> LosAngeles.Streets.description
        | _ -> failwith "How did you get here so fast?!"

    zone.Descriptors
    |> List.map (cityGenerator currentDayMoment)
    |> List.sample
    |> String.concat " "
