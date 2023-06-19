namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities
open Duets.Data.World
open Duets.Simulation

module World =
    /// Returns all cities available in the game world.
    let allCities =
        World.get () |> Optic.get Lenses.World.cities_ |> List.ofMapValues

    /// Returns a specific city given its ID.
    let cityById cityId =
        World.get ()
        |> Optic.get (Lenses.World.city_ cityId)
        |> Option.get (* Not finding a city by its ID is a problem in city creation. *)

    /// Returns a place inside a given city by its ID.
    let placeInCityById cityId placeId =
        let city = cityById cityId
        Map.find placeId city.PlaceIndex

    /// Returns a place inside of the current city given its ID.
    let placeInCurrentCityById state placeId =
        let cityId, _, _ = state.CurrentPosition
        placeInCityById cityId placeId

    /// Returns all the places in the current city, organized by their place type.
    let allPlacesInCurrentCity state =
        let cityId, _, _ = state.CurrentPosition
        let city = cityById cityId

        city.PlaceByTypeIndex
        |> Map.map (fun _ placeIds ->
            placeIds |> List.map (placeInCityById cityId))

    /// Returns the current world coordinates the character is in currently.
    let currentCoordinates state = state.CurrentPosition

    /// Returns the city in which the character is in currently.
    let currentCity state =
         let cityId, _, _ = state.CurrentPosition
         cityId |> cityById

    /// Returns the place in which the character is in currently.
    let currentPlace state =
        let _, placeId, _ = state.CurrentPosition
        placeInCurrentCityById state placeId

    /// Returns a list of IDs of the places with the given type inside of the
    /// given city.
    let placeIdsByTypeInCity cityId placeType =
        cityById cityId
        |> Optic.get (
            Lenses.World.City.placeByTypeIndex_ >-> Map.key_ placeType
        )
        |> Option.defaultValue []

    /// Returns a list of places with the given type inside of a city.
    let placesByTypeInCity cityId placeType =
        placeIdsByTypeInCity cityId placeType
        |> List.map (placeInCityById cityId)

    /// Returns the distance between the given cities.
    let distanceBetween city1 city2 =
        let connection = World.connectionBetween city1 city2

        World.distances |> Map.find connection

    /// Returns whether the given place is currently open or not.
    let placeCurrentlyOpen place currentTime =
        let currentDayMoment = Calendar.Query.dayMomentOf currentTime

        match place.OpeningHours with
        | PlaceOpeningHours.AlwaysOpen -> true
        | PlaceOpeningHours.OpeningHours (daysOfWeekOpen, dayMomentsOpen) ->
            (daysOfWeekOpen |> List.contains currentTime.DayOfWeek
             && dayMomentsOpen |> List.contains currentDayMoment)

    /// Like `placeCurrentlyOpen` but implicitly passing the current time.
    let placeCurrentlyOpen' state place =
        Queries.Calendar.today state |> placeCurrentlyOpen place
