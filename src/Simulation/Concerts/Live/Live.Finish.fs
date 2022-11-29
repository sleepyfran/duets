module Simulation.Concerts.Live.Finish

open Common
open Entities
open Entities.SituationTypes
open Simulation

/// Creates a ConcertFinished effect which adds the concert to the history of
/// the band and stops them from being able to perform in the venue for the day.
let finishConcert state ongoingConcert =
    let band = Queries.Bands.currentBand state

    let fanGain =
        match ongoingConcert.Points with
        | p when p <= 40<quality> ->
            float ongoingConcert.Concert.TicketsSold
            * Config.MusicSimulation.concertLowPointFanDecreaseRate
        | p when p <= 65<quality> ->
            float ongoingConcert.Concert.TicketsSold
            * Config.MusicSimulation.concertMediumPointFanIncreaseRate
        | p when p <= 85<quality> ->
            float ongoingConcert.Concert.TicketsSold
            * Config.MusicSimulation.concertGoodPointFanIncreaseRate
        | _ ->
            float ongoingConcert.Concert.TicketsSold
            * Config.MusicSimulation.concertHighPointFanIncreaseRate
        |> Math.ceilToNearest

    let updatedFans =
        band.Fans + fanGain |> Math.lowerClamp 0

    [ ConcertFinished(
          band,
          PerformedConcert(ongoingConcert.Concert, ongoingConcert.Points)
      )
      BandFansChanged(band, Diff(band.Fans, updatedFans))
      SituationChanged FreeRoam ]
