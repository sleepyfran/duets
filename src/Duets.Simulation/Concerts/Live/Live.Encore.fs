module Duets.Simulation.Concerts.Live.Encore

open Duets.Common
open Duets.Data.World
open Duets.Entities
open Duets.Simulation.Navigation

/// Moves the character to the backstage, checking whether an encore can or not
/// be performed. In the event of not being able to perform encore, we finish
/// the concert and return whether an encore is possible or not and the coordinates
/// to the backstage.
let getOffStage state ongoingConcert =
    let canPerformEncore = Concert.Ongoing.canPerformEncore ongoingConcert

    let situationEffects =
        if not canPerformEncore then
            Finish.finishConcert state ongoingConcert
        else
            state
            |> Navigation.enter Ids.ConcertSpace.backstage
            |> Result.unwrap
            |> List.ofItem

    let updatedConcert = ongoingConcert |> addEvent GetOffStage

    [ ConcertActionPerformed(GetOffStage, updatedConcert, Done, 0<points>)
      ConcertGotOffStage canPerformEncore ]
    @ situationEffects

/// Adds a new encore to the list of events of the ongoing concert.
let doEncore state ongoingConcert =
    let effects =
        [ state |> Navigation.enter Ids.ConcertSpace.stage |> Result.unwrap ]

    let updatedConcert = ongoingConcert |> addEvent PerformedEncore

    ConcertActionPerformed(PerformedEncore, updatedConcert, Done, 0<points>)
    :: effects
