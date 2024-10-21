module Duets.Simulation.Concerts.Live.Finish

open Aether
open Duets.Common
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation
open Duets.Simulation.Bank
open Duets.Simulation.Time

let private calculateFanGain ongoingConcert =
    let participationFactor =
        match ongoingConcert.Concert.ParticipationType with
        | Headliner -> 1.0
        | OpeningAct _ -> 0.2

    let qualityFactor =
        match ongoingConcert.Points with
        | p when p <= 40<quality> ->
            Config.MusicSimulation.concertLowPointFanDecreaseRate
        | p when p <= 65<quality> ->
            Config.MusicSimulation.concertMediumPointFanIncreaseRate
        | p when p <= 85<quality> ->
            Config.MusicSimulation.concertGoodPointFanIncreaseRate
        | _ -> Config.MusicSimulation.concertHighPointFanIncreaseRate

    float ongoingConcert.Concert.TicketsSold
    * qualityFactor
    * participationFactor
    |> Math.ceilToNearest
    |> (*) 1<fans>

let private calculateEarnings ongoingConcert =
    let earningPercentage =
        match ongoingConcert.Concert.ParticipationType with
        | Headliner -> 1.0
        | OpeningAct(_, earningPercentage) ->
            let percent = earningPercentage / 1<percent> |> float
            percent / 100.0
        |> decimal

    let concertSpaceCut =
        ongoingConcert.Concert.VenueId
        |> Queries.World.placeInCityById ongoingConcert.Concert.CityId
        |> Queries.Concerts.concertSpaceTicketPercentage
        |> decimal

    let totalEarnings =
        decimal ongoingConcert.Concert.TicketsSold
        * ongoingConcert.Concert.TicketPrice
        * earningPercentage

    let venueCut = totalEarnings * concertSpaceCut

    totalEarnings - venueCut

/// Creates a ConcertFinished effect which adds the concert to the history of
/// the band and stops them from being able to perform in the venue for the day.
let finishConcert state ongoingConcert =
    let band = Queries.Bands.currentBand state
    let concertCity = ongoingConcert.Concert.CityId
    let fansInCity = Queries.Bands.fansInCity' band concertCity

    let updatedFansInCity =
        calculateFanGain ongoingConcert + fansInCity |> Math.lowerClamp 0<fans>

    let updatedFans = Map.add concertCity updatedFansInCity band.Fans

    let bandAccount = Band band.Id
    let concertEarnings = calculateEarnings ongoingConcert

    [ ConcertFinished(
          band,
          PerformedConcert(ongoingConcert.Concert, ongoingConcert.Points),
          concertEarnings
      )
      BandFansChanged(band, Diff(band.Fans, updatedFans))
      Operations.income state bandAccount concertEarnings
      SituationChanged FreeRoam ]
