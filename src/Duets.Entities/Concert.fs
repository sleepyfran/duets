module Duets.Entities.Concert

open Aether
open Duets.Entities

type TicketPriceError =
    | PriceBelowZero
    | PriceTooHigh

module Timeline =
    let empty =
        { ScheduledEvents = Set.empty
          PastEvents = Set.empty }

module Ongoing =
    /// Returns the number of times that an event was performed.
    let timesDoneEvent ongoingConcert event =
        Optic.get Lenses.Concerts.Ongoing.events_ ongoingConcert
        |> List.filter (fun performedEvent ->
            match performedEvent with
            | PlaySong(playedSong, _) ->
                match event with
                | PlaySong(song, _) -> playedSong = song
                | _ -> false
            | _ -> performedEvent = event)
        |> List.length
        |> (*) 1<times>

    /// Returns whether the given song has been previously played in the concert
    /// or not.
    let hasPlayedSong ongoingConcert song =
        timesDoneEvent ongoingConcert (PlaySong(song, Energetic)) > 0<times>

    /// Returns whether the band has accumulated enough points during the concert
    /// for people to be interested in an encore and not just leave immediately
    /// the moment you leave the stage.
    let canPerformEncore ongoingConcert =
        let timesPerformedEncores =
            timesDoneEvent ongoingConcert PerformedEncore

        let points = Optic.get Lenses.Concerts.Ongoing.points_ ongoingConcert

        points > 50<quality> && timesPerformedEncores = 0<times>

/// Creates a concert from the given parameter.
let create date dayMoment cityId venueId ticketPrice participationType =
    let ticketAmount = ticketPrice |> Amount.fromDecimal

    { Id = Identity.create ()
      CityId = cityId
      VenueId = venueId
      Date = date
      DayMoment = dayMoment
      TicketPrice = ticketAmount
      TicketsSold = 0
      ParticipationType = participationType }

/// Validates that the ticket price is not below zero or a too high number.
let validatePrice ticketPrice =
    if ticketPrice < 0m then Error PriceBelowZero
    else if ticketPrice > 10000m then Error PriceTooHigh
    else Ok ticketPrice

/// Returns the inner concert inside a past concert.
let fromPast (concert: PastConcert) =
    match concert with
    | PerformedConcert(concert, _) -> concert
    | FailedConcert(concert, _) -> concert

/// Returns the inner concert inside a scheduled concert.
let fromScheduled (ScheduledConcert(concert, _)) = concert
