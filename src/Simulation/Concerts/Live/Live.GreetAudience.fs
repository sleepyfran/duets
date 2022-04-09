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

    let timesGreetedAudience =
        Concert.Ongoing.timesDoneEvent ongoingConcert event

    match timesGreetedAudience with
    | times when times >= 1 ->
        Response.forEvent' ongoingConcert event -10 GreetedMoreThanOnce
    | _ -> Response.forEvent' ongoingConcert event 5 Ok
