module rec Duets.Simulation.Concerts.OpeningActOpportunities

open Duets.Common
open Duets.Common.Operators
open Duets.Entities
open Duets.Simulation

/// Generates a list of opportunities for a band to be an opening act.
let generate state cityId =
    let today = Queries.Calendar.today state

    // Start from a week after so that there's time to gather ticket sales.
    let firstAvailableDay = today |> Calendar.Ops.addDays 7
    let lastAvailableDay = today |> Calendar.Ops.addMonths 1

    let simulatedBands = Queries.Bands.allSimulated state |> List.ofMapValues

    Calendar.Query.datesBetween firstAvailableDay lastAvailableDay
    |> List.collect (generateOpeningActShowsOnDate state simulatedBands cityId)

let private generateOpeningActShowsOnDate state headlinerBands cityId date =
    let eventsToGenerate = RandomGen.genBetween 0 15

    let venuesInCity =
        Queries.World.placesByTypeInCity cityId PlaceTypeIndex.ConcertSpace

    [ 0..eventsToGenerate ]
    |> List.map (fun _ ->
        let dayMoment = [ Evening; Night ] |> List.sample
        let venue = venuesInCity |> List.sample
        let headliner = headlinerBands |> List.sample
        let ticketPrice = Queries.Concerts.fairTicketPrice state headliner

        let concert =
            Concert.create
                date
                dayMoment
                cityId
                venue.Id
                ticketPrice
                (OpeningAct headliner.Id)

        (headliner, concert))

type OpeningActApplicationError =
    | NotEnoughFame
    | NotEnoughReleases
    | AnotherConcertAlreadyScheduled
    | GenreMismatch

let private (|LacksFame|_|) state headliner band =
    let bandFame = Queries.Bands.estimatedFameLevel state band
    let headlinerFame = Queries.Bands.estimatedFameLevel state headliner

    if bandFame >=< (headlinerFame - 25, 100) then
        None
    else
        Some()

let private (|LacksReleases|_|) state _ (band: Band) =
    Queries.Albums.releasedByBand state band.Id |> List.isEmpty |> Option.ofBool

let private (|HasOtherConcertsOnDate|_|) state concert _ =
    Scheduler.validateNoOtherConcertsInDate state concert.Date
    |> Result.isError
    |> Option.ofBool

// TODO: Implement once we have genre matching in the game.
let private (|MismatchesGenre|_|) _ _ _ : unit option = None

/// Applies to an opening act opportunity. Checks if the band is a good fit for
/// the headliner and if so, schedules the concert. Otherwise, returns an error
/// specifying why the band was not a good fit.
let applyToConcertOpportunity state headliner concert =
    let currentBand = Queries.Bands.currentBand state

    match currentBand with
    | LacksFame state headliner -> Error NotEnoughFame
    | LacksReleases state headliner -> Error NotEnoughReleases
    | HasOtherConcertsOnDate state concert ->
        Error AnotherConcertAlreadyScheduled
    | MismatchesGenre state headliner -> Error GenreMismatch
    | _ ->
        let today = Queries.Calendar.today state
        ConcertScheduled(currentBand, ScheduledConcert(concert, today)) |> Ok
