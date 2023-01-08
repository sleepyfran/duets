module Simulation.Navigation.Policies.OpeningHours

open Common
open Entities
open Simulation

/// Returns whether the given place is opened right now according to their
/// opening hours.
let canEnter state cityId placeId =
    let place = (cityId, placeId) ||> Queries.World.placeInCityById

    Queries.World.placeCurrentlyOpen' state place
    |> Result.ofBool PlaceEntranceError.CannotEnterOutsideOpeningHours
