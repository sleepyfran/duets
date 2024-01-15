module Duets.Simulation.Events.Concert

open Duets.Entities
open Duets.Simulation

/// Runs all the events associated with effects of a concert. For example,
/// finishing a concert will start an event to calculate merch sales for that
/// specific concert.
let internal run effect =
    match effect with
    | ConcertFinished(band, pastConcert, income) ->
        [ Merchandise.Sell.afterConcert band pastConcert ]
        |> ContinueChain
        |> Some
    | _ -> None
