module Duets.Simulation.Events.Moodlets.Moodlets

open Duets.Entities
open Duets.Simulation.Events

/// Runs all the events associated with moodlets. For example, when a band
/// composes more than two songs in a week, a "NotInspired" moodlet is given
/// to the character for another week to slow them down a bit.
let internal run effect =
    match effect with
    | ConcertFinished(band, _, _) ->
        ContinueChain [ TiredOfTouring.applyIfNeeded band.Id ] |> Some
    | SongFinished(band, _, _) ->
        ContinueChain [ NotInspired.applyIfNeeded band.Id ] |> Some
    | TimeAdvanced _ -> ContinueChain [ Cleanup.cleanup ] |> Some
    | WorldMoveTo(Diff((prevCityId, _, _), (currCityId, _, _))) ->
        ContinueChain [ JetLagged.applyIfNeeded prevCityId currCityId ] |> Some
    | _ -> None
