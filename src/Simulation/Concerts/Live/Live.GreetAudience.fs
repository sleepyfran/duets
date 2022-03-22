[<AutoOpen>]
module Simulation.Concerts.Live.GreetAudience

open Entities

type GreetAudienceResult =
    | GreetedMoreThanOnce
    | Ok

/// Greets the audience and returns the result. If the audience has been greeted
/// already then it returns `GreetedMoreThanOnce`.
let greetAudience ongoingConcert =
    let event = CommonEvent GreetAudience

    match Concert.Ongoing.timesGreetedAudience ongoingConcert with
    | times when times >= 1 ->
        response ongoingConcert event -10 GreetedMoreThanOnce
    | _ -> response ongoingConcert event 5 Ok
