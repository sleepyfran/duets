module Duets.Simulation.Navigation.Policies.Concert

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Returns whether the character has a concert scheduled in the current venue
/// and it's right in this day moment and therefore can access or not the stage.
let private canEnterStage state placeId =
    let timeRightNow = Queries.Calendar.today state

    let band = Queries.Bands.currentBand state

    Queries.Concerts.scheduleForTodayInPlace state band.Id placeId
    |> Option.map (fun (ScheduledConcert(concert, _)) ->
        let dateWithDayMoment =
            concert.Date
            |> Calendar.Transform.changeDayMoment concert.DayMoment

        dateWithDayMoment = timeRightNow)
    |> Option.defaultValue false
    |> Result.ofBool RoomEntranceError.CannotEnterStageOutsideConcert

/// Returns whether the character has a concert scheduled around the time
/// they're attempting to enter the backstage.
let private canEnterBackstage state placeId =
    let band = Queries.Bands.currentBand state

    Queries.Concerts.scheduledAroundDate state band.Id
    |> List.exists (fun concert -> concert.VenueId = placeId)
    |> Result.ofBool RoomEntranceError.CannotEnterBackstageOutsideConcert

/// Returns whether it's possible to enter in the given coordinates.
let canEnter state cityId placeId roomId =
    let place = Queries.World.placeInCityById cityId placeId
    let room = Queries.World.roomById cityId placeId roomId

    match place.PlaceType with
    | ConcertSpace _ when room.RoomType = RoomType.Stage ->
        canEnterStage state placeId
    | ConcertSpace _ when room.RoomType = RoomType.Backstage ->
        canEnterBackstage state placeId
    | _ -> Ok()
