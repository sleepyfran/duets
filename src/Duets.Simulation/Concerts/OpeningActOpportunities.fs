module rec Duets.Simulation.Concerts.OpeningActOpportunities

open Duets.Common
open Duets.Common.Operators
open Duets.Entities
open Duets.Simulation

/// Generates a list of opportunities for a band to be an opening act.
let generate state cityId =
    let today = Queries.Calendar.today state
    let currentBand = Queries.Bands.currentBand state

    // Start from a week after so that there's time to gather ticket sales.
    let firstAvailableDay = today |> Calendar.Ops.addDays 7
    let lastAvailableDay = today |> Calendar.Ops.addMonths 1

    let simulatedBands =
        Queries.Bands.allSimulated state
        |> List.ofMapValues
        |> List.filter (fun headliner ->
            let headlinerFame =
                Queries.Bands.estimatedFameLevel state headliner

            let currentBandFame =
                Queries.Bands.estimatedFameLevel state currentBand

            headlinerFame >=< (0, currentBandFame + 35))

    Calendar.Query.datesBetween firstAvailableDay lastAvailableDay
    |> List.collect (generateOpeningActShowsOnDate state simulatedBands cityId)

let private generateOpeningActShowsOnDate state headlinerBands cityId date =
    let eventsToGenerate = RandomGen.genBetween 0 15

    let venuesInCity =
        Queries.World.placesByTypeInCity cityId PlaceTypeIndex.ConcertSpace

    [ 0..eventsToGenerate ]
    |> List.map (fun _ ->
        let dayMoment = [ Evening; Night ] |> List.sample
        let headliner = headlinerBands |> List.sample
        let ticketPrice = Queries.Concerts.fairTicketPrice state headliner

        let headlinerFameLevel =
            Queries.Bands.estimatedFameLevel state headliner

        let earningPercentage = calculateEarningPercentage headlinerFameLevel

        let venue = findSuitableVenue venuesInCity headliner.Fans

        let concert =
            Concert.create
                date
                dayMoment
                cityId
                venue.Id
                ticketPrice
                (OpeningAct(headliner.Id, earningPercentage))

        (headliner, concert))

let private findSuitableVenue venuesInCity fans : Place =
    let range =
        match fans with
        | fans when fans <= 1000 -> (0, 300)
        | fans when fans <= 5000 -> (0, 500)
        | fans when fans <= 20000 -> (500, 5000)
        | fans when fans <= 100000 -> (500, 20000)
        | _ -> (500, 500000)

    (*
    We rely on the fact that there will always be a suitable venue in the city.
    *)
    venuesInCity
    |> List.filter (fun venue ->
        let capacity =
            match venue.PlaceType with
            | PlaceType.ConcertSpace concertSpace -> concertSpace.Capacity
            | _ -> 0

        capacity >=< range)
    |> List.sample

let private calculateEarningPercentage headlinerFame =
    match headlinerFame with
    | level when level < 30 -> 20<percent>
    | level when level < 50 -> 15<percent>
    | level when level < 70 -> 10<percent>
    | _ -> 5<percent>

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
