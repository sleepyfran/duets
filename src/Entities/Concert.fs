module Entities.Concert

open Aether
open Entities

type TicketPriceError =
    | PriceBelowZero
    | PriceTooHigh

module Timeline =
    let empty =
        { ScheduledEvents = Set.empty
          PastEvents = Set.empty }

module Ongoing =
    /// Returns whether the given song has already being played in the passed
    /// ongoing concert.
    let hasPlayedSong ongoingConcert song =
        Optic.get Lenses.Concerts.Ongoing.events_ ongoingConcert
        |> List.exists
            (fun event ->
                match event with
                | CommonEvent commonEvent ->
                    match commonEvent with
                    | PlaySong (playedSong, _) -> playedSong = song
                    | _ -> false
                | _ -> false)

    /// Returns the number of times that an event was performed. Do NOT use with
    /// events that have inner fields that we do not want to match such as
    /// PlaySong (contains perform energy which is not used in the comparison),
    /// since it might not return the correct number. Use specific functions
    /// for those.
    let timesDoneEvent ongoingConcert event =
        Optic.get Lenses.Concerts.Ongoing.events_ ongoingConcert
        |> List.filter ((=) event)
        |> List.length

    /// Returns whether the band has accumulated enough points during the concert
    /// for people to be interested in an encore and not just leave immediately
    /// the moment you leave the stage.
    let canPerformEncore ongoingConcert =
        let timesPerformedEncores =
            timesDoneEvent ongoingConcert (CommonEvent PerformedEncore)

        let points =
            Optic.get Lenses.Concerts.Ongoing.points_ ongoingConcert

        points > 50<quality> && timesPerformedEncores = 0

/// Creates a concert from the given parameter.
let create date dayMoment cityId venueId ticketPrice =
    let ticketAmount = ticketPrice * 1<dd>

    { Id = Identity.create ()
      CityId = cityId
      VenueId = venueId
      Date = date
      DayMoment = dayMoment
      TicketPrice = ticketAmount
      TicketsSold = 0 }

/// Validates that the ticket price is not below zero or a too high number.
let validatePrice ticketPrice =
    if ticketPrice < 0 then
        Error PriceBelowZero
    else if ticketPrice > 10000 then
        Error PriceTooHigh
    else
        Ok ticketPrice

/// Returns the inner concert inside a past concert.
let fromPast (concert: PastConcert) =
    match concert with
    | PerformedConcert (concert, _) -> concert
    | FailedConcert concert -> concert

/// Returns the inner concert inside a scheduled concert.
let fromScheduled (ScheduledConcert concert) = concert
