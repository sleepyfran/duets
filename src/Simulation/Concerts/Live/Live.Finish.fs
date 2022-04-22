module Simulation.Concerts.Live.Finish

open Entities
open Simulation

/// Creates a ConcertFinished effect which adds the concert to the history of
/// the band and stops them from being able to perform in the venue for the day.
let finishConcert state ongoingConcert =
    let band = Queries.Bands.currentBand state

    [ ConcertFinished(
          band,
          PerformedConcert(ongoingConcert.Concert, ongoingConcert.Points)
      )
      SituationChanged FreeRoam ]
