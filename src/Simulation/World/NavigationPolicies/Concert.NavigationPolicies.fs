module Simulation.World.NavigationPolicies.Concert

open Common
open Entities
open Simulation

/// Returns whether the character has a concert scheduled in the current venue
/// and it's right in this day moment and therefore can access or not the stage.
let private canEnterStage state (coords: ResolvedPlaceCoordinates) =
    let placeId, _ = coords.Coordinates

    let timeRightNow =
        Queries.Calendar.today state

    let band = Queries.Bands.currentBand state

    Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
    |> Option.map (fun (ScheduledConcert concert) ->
        let dateWithDayMoment =
            concert.Date
            |> Calendar.Transform.changeDayMoment concert.DayMoment

        dateWithDayMoment = timeRightNow)
    |> Option.defaultValue false
    |> Result.ofBool EntranceError.CannotEnterStageOutsideConcert

/// Returns whether the character has a concert scheduled around the time
/// they're attempting to enter the backstage.
let private canEnterBackstage state (coords: ResolvedPlaceCoordinates) =
    let placeId, _ = coords.Coordinates

    let band = Queries.Bands.currentBand state

    Queries.Concerts.scheduledAroundDate state band.Id
    |> List.exists (fun concert -> concert.VenueId = placeId)
    |> Result.ofBool EntranceError.CannotEnterBackstageOutsideConcert

/// Returns whether it's possible to enter in the given coordinates.
let canEnter state (coords: ResolvedPlaceCoordinates) =
    match coords.Place.Space with
    | ConcertSpace _ ->
        match coords.Room with
        | Room.Stage -> canEnterStage state coords
        | Room.Backstage -> canEnterBackstage state coords
        | _ -> Ok()
    | _ -> Ok()
