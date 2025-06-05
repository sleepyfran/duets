namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

module Rentals =
    /// Returns a map of all current rentals per city.
    let allAsMap state = Optic.get Lenses.State.rentals_ state

    /// Returns a list of all current rentals for all cities.
    let all state = allAsMap state |> List.ofMapValues

    /// Returns a list of all upcoming monthly rental payments within this week.
    let allUpcoming state =
        let nextWeekDate = Calendar.today state |> Calendar.Ops.addDays 7<days>

        all state
        |> List.filter (fun rental ->
            match rental.RentalType with
            | Monthly nextPaymentDate -> nextPaymentDate <= nextWeekDate
            | _ -> false)

    /// Returns an optional rental for a place given its coordinates.
    let getForCoords state coords =
        Optic.get (Lenses.State.rentals_ >-> Map.value_ coords) state

    /// Calculates the price for renting a given place.
    let calculateMonthlyRentalPrice cityId (place: Place) =
        let city = World.cityById cityId

        let fullPrice =
            match place.PlaceType with
            | PlaceType.Home -> decimal city.CostOfLiving * 500m<dd>
            | _ -> 0m<dd>

        let qualityModifier = decimal (place.Quality / 1<quality>) / 100m

        fullPrice * (decimal qualityModifier)

    /// Calculates the price for renting a given place for a given number of
    /// days.
    let calculateOneTimeRentalPrice (place: Place) (numberOfDays: int<days>) =
        match place.PlaceType with
        | PlaceType.Hotel hotel -> hotel.PricePerNight * decimal numberOfDays
        | _ -> 0m<dd>

    /// Returns all places of the given type inside of the city that have still
    /// not been rented.
    let placesAvailableForRentInCity state cityId placeType =
        World.placesByTypeInCity cityId placeType
        |> List.filter (fun place ->
            (cityId, place.Id) |> getForCoords state |> Option.isNone)
