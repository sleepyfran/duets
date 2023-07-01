module Duets.Simulation.Events.Band.Reviews

open Duets.Simulation.Albums

let generateReviewsAfterFanIncrease bandId state =
    ReviewGeneration.generateReviewsForBand state bandId
