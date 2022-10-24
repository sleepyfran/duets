namespace Simulation.Queries

open Aether
open Aether.Operators
open Common
open Entities
open Data.World

module World =
    /// Returns all cities available in the game world.
    let allCities =
        World.get ()
        |> Optic.get Lenses.World.cities_
        |> List.ofMapValues

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
        let cityId, _ = state.CurrentPosition
        placeInCityById cityId placeId

    /// Returns the city in which the character is in currently.
    let currentCity state = fst state.CurrentPosition |> cityById

    /// Returns the place in which the character is in currently.
    let currentPlace state =
        let _, placeId = state.CurrentPosition
        placeInCurrentCityById state placeId

    /// Returns a list of IDs of the places with the given type inside of the
    /// given city.
    let placeIdsOf cityId placeType =
        cityById cityId
        |> Optic.get (
            Lenses.World.City.placeByTypeIndex_
            >-> Map.key_ placeType
        )
        |> Option.defaultValue []
