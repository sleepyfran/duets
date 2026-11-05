[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Highlights

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

/// Defines why a place on the street is relevant to the player.
type Highlight =
    | Rented
    | ConcertToday of Concert
    | ConcertScheduled of Concert
    | Workplace

let private isRented state cityId (place: Place) =
    Queries.Rentals.getForCoords state (cityId, place.Id) |> Option.isSome

let private nextConcert state cityId (place: Place) =
    let today = Queries.Calendar.today state |> Calendar.Transform.resetDayMoment

    Queries.Bands.currentBandId state
    |> Queries.Concerts.allScheduled state
    |> List.map Concert.fromScheduled
    |> List.filter (fun concert ->
        concert.CityId = cityId && concert.VenueId = place.Id)
    |> List.sortBy _.Date
    |> List.tryHead
    |> Option.map (fun concert ->
        if Calendar.Transform.resetDayMoment concert.Date = today then
            ConcertToday concert
        else
            ConcertScheduled concert)

let private isWorkplace state cityId (place: Place) =
    Queries.Career.current state
    |> Option.exists (fun job ->
        let jobCityId, jobPlaceId, _ = job.Location
        jobCityId = cityId && jobPlaceId = place.Id)

/// Returns all the reasons why the given place is relevant to the player.
let forPlace state cityId (place: Place) =
    [ if isRented state cityId place then
          Rented
      yield! nextConcert state cityId place |> Option.toList
      if isWorkplace state cityId place then
          Workplace ]

/// Returns the glyph and style used to mark the given highlight in the scene.
let markerOf highlight =
    match highlight with
    | Rented -> SceneStyles.Highlight.rented, SceneStyles.Highlight.rentedColor
    | ConcertToday _ ->
        SceneStyles.Highlight.concert, SceneStyles.Highlight.concertTodayColor
    | ConcertScheduled _ ->
        SceneStyles.Highlight.concert,
        SceneStyles.Highlight.concertScheduledColor
    | Workplace ->
        SceneStyles.Highlight.workplace, SceneStyles.Highlight.workplaceColor
