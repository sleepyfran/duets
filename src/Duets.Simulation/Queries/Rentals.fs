namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

module Rentals =
    /// Returns a list of all current rentals for all cities.
    let all state =
        Optic.get Lenses.State.rentals_ state |> List.ofMapValues

    /// Returns a list of all upcoming monthly rental payments within this week.
    let allUpcoming state =
        let nextWeekDate = Calendar.today state |> Calendar.Ops.addDays 7

        all state
        |> List.filter (fun rental ->
            match rental.RentalType with
            | Monthly nextPaymentDate -> nextPaymentDate <= nextWeekDate
            | _ -> false)

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
