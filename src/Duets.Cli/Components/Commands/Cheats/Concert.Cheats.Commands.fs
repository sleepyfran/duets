namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts
open Aether

[<RequireQualifiedAccess>]
module ConcertCommands =
    /// Schedules a headliner concert for this evening in the current city (or
    /// tomorrow evening if it's already evening or later), using the fair
    /// ticket price and a randomly chosen venue.
    let scheduleNightConcert =
        { Name = "book a gig"
          Description =
            "Schedules a concert for tonight (or tomorrow evening if it's already evening or later) in the current city"
          Handler =
            (fun _ ->
                let state = State.get ()
                let cityId, _, _ = Queries.World.currentCoordinates state
                let today = Queries.Calendar.today state
                let currentDayMoment = Calendar.Query.dayMomentOf today

                let concertDate, concertDayMoment =
                    match currentDayMoment with
                    | Evening
                    | Night
                    | Midnight ->
                        Calendar.Ops.addDay today
                        |> Calendar.Transform.changeDayMoment Evening,
                        Evening
                    | _ -> Calendar.Transform.changeDayMoment Evening today, Evening

                let venues =
                    Queries.World.placesByTypeInCity
                        cityId
                        PlaceTypeIndex.ConcertSpace

                match venues with
                | [] ->
                    "There are no concert venues in the current city!"
                    |> Styles.error
                    |> showMessage

                    Scene.Cheats
                | _ ->
                    let venue = List.sample venues

                    let ticketPrice =
                        Queries.Concerts.fairTicketPrice state (Queries.Bands.currentBandId state)

                    Scheduler.scheduleHeadlinerConcert
                        state
                        concertDate
                        concertDayMoment
                        cityId
                        venue.Id
                        ticketPrice
                    |> Effect.apply

                    $"Booked a gig at {venue.Name |> Styles.place} for this evening!"
                    |> Styles.success
                    |> showMessage

                    Scene.Cheats) }

    /// Allows the player to choose one of the scheduled concerts and set how
    /// many tickets have been sold for it, up to the venue's capacity.
    let soldOut =
        { Name = "sold out"
          Description =
            "Allows you to choose how many tickets have been sold for one of your scheduled concerts"
          Handler =
            (fun _ ->
                let state = State.get ()
                let band = Queries.Bands.currentBand state

                let concertText scheduledConcert =
                    let concert = Concert.fromScheduled scheduledConcert

                    let venue =
                        Queries.World.placeInCityById
                            concert.CityId
                            concert.VenueId

                    $"""{venue.Name |> Styles.place}, {Generic.cityName concert.CityId |> Styles.place} on {Date.simple concert.Date} {Styles.faded $"({concert.TicketsSold} tickets sold)"}"""

                let selectedConcert =
                    Queries.Concerts.allScheduled state band.Id
                    |> showOptionalChoicePrompt
                        "Which concert do you want to sell tickets for?"
                        Generic.back
                        concertText

                match selectedConcert with
                | Some scheduledConcert ->
                    let concert = Concert.fromScheduled scheduledConcert

                    let venue =
                        Queries.World.placeInCityById
                            concert.CityId
                            concert.VenueId

                    let capacity =
                        match venue.PlaceType with
                        | PlaceType.ConcertSpace concertSpace ->
                            concertSpace.Capacity
                        | _ -> 0

                    let ticketsSold =
                        $"You've sold {concert.TicketsSold} tickets so far, how many do you want to have sold?"
                        |> Styles.prompt
                        |> showRangedNumberPrompt 0 capacity

                    let updatedConcert =
                        Optic.set
                            Lenses.Concerts.Scheduled.ticketsSold_
                            ticketsSold
                            scheduledConcert

                    ConcertUpdated(band, updatedConcert) |> Effect.apply

                    $"Done! {ticketsSold} tickets sold for the concert at {venue.Name |> Styles.place}"
                    |> Styles.success
                    |> showMessage
                | None -> ()

                Scene.Cheats) }
