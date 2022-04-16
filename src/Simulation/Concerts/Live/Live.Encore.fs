module Simulation.Concerts.Live.Encore

open Entities
open Simulation

/// Moves the character to the backstage, checking whether an encore can or not
/// be performed. In the event of not being able to perform encore, we finish
/// the concert and return whether an encore is possible or not and the coordinates
/// to the backstage.
let getOffStage state ongoingConcert =
    let backstageCoordinates =
        Queries.World.ConcertSpace.closestBackstage state
        |> Option.get // Not having a backstage is a problem in city creation.

    let navigationEffects =
        [ World.Navigation.moveTo backstageCoordinates state ]

    let canPerformEncore =
        Concert.Ongoing.canPerformEncore ongoingConcert

    let backstageRoomId =
        Queries.World.Common.roomIdFromCoordinates backstageCoordinates
        |> Option.get

    let finishedConcertEffects =
        if not canPerformEncore then
            [ Finish.finishConcert state ongoingConcert ]
        else
            []

    Response.forEvent ongoingConcert GotOffStage 0
    |> Response.addEffects (navigationEffects @ finishedConcertEffects)
    |> Response.mapResult (fun _ -> (canPerformEncore, backstageRoomId))

/// Adds a new encore to the list of events of the ongoing concert.
let doEncore state ongoingConcert =
    let stageCoordinates =
        Queries.World.ConcertSpace.closestStage state
        |> Option.get // Not having a stage is a problem in city creation.

    let navigationEffects =
        [ World.Navigation.moveTo stageCoordinates state ]

    Response.forEvent ongoingConcert PerformedEncore 0
    |> Response.addEffects navigationEffects
