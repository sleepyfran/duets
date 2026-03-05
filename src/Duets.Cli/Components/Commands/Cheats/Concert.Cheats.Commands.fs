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
