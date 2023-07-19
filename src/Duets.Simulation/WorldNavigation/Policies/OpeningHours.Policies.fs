module Duets.Simulation.Navigation.Policies.OpeningHours

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns whether the given place is opened right now according to their
/// opening hours.
let canMove state cityId placeId =
    let place = (cityId, placeId) ||> Queries.World.placeInCityById

    Queries.World.placeCurrentlyOpen' state place
    |> Result.ofBool PlaceEntranceError.CannotEnterOutsideOpeningHours
