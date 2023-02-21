namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Entities

module Rentals =
    /// Returns an optional rental for a place given its coordinates.
    let getForCoords state coords =
        Optic.get (Lenses.State.rentals_ >-> Map.value_ coords) state

    /// Calculates the price for renting a given place.
    let calculateRentalPrice cityId (place: Place) =
        let city = World.cityById cityId

        let fullPrice =
            match place.Type with
            | PlaceType.Home -> decimal city.PlaceCostModifier * 500m<dd>
            | _ -> 0m<dd>

        let qualityModifier = decimal (place.Quality / 1<quality>) / 100m

        fullPrice * (decimal qualityModifier)

    /// Returns all places of the given type inside of the city that have still
    /// not been rented.
    let placesAvailableForRentInCity state cityId placeType =
        World.placesByTypeInCity cityId placeType
        |> List.filter (fun place ->
            (cityId, place.Id) |> getForCoords state |> Option.isNone)
