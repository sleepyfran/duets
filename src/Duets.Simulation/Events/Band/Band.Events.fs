module Duets.Simulation.Events.Band.Band

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Events

/// Runs all the events associated with bands. For example, when the fan base
/// changes, the engine might generate new reviews for their albums.
let internal run effect =
    match effect with
    | BandFansChanged(band, Diff(prevFans, currentFans)) when
        prevFans < Config.MusicSimulation.minimumFanBaseForReviews
        && currentFans >= Config.MusicSimulation.minimumFanBaseForReviews
        ->
        [ Reviews.generateReviewsAfterFanIncrease band.Id ]
        |> ContinueChain
        |> Some
    | _ -> None
