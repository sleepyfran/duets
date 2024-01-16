[<AutoOpen>]
module Duets.Cli.Components.ConcertDetails

open Duets.Agents
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Spectre.Console

/// Shows a social network post posted by the given account.
let showConcertDetails (concert: Concert) =
    let state = State.get ()
    let place = Queries.World.placeInCityById concert.CityId concert.VenueId
    let cityName = Generic.cityName concert.CityId
    let band = Queries.Bands.currentBand state

    let title =
        match concert.ParticipationType with
        | Headliner -> band.Name |> Styles.title
        | OpeningAct(headlinerId, _) ->
            let headliner = Queries.Bands.byId state headlinerId
            $"{headliner.Name |> Styles.title}, support: {band.Name |> Styles.faded}"

    lineBreak ()

    Panel(
        Rows(
            Markup(title),
            Markup($"Venue: {place.Name |> Styles.place}, {cityName}"),
            Markup(
                $"Scheduled for {concert.Date |> Generic.dateWithDay} at {concert.DayMoment |> Generic.dayMomentName |> String.lowercase}"
            )
        ),
        Header =
            PanelHeader($"{Emoji.concert} Concert scheduled" |> Styles.header),
        Border = BoxBorder.Rounded,
        Expand = true
    )
    |> AnsiConsole.Write

    showTips
        [ "Make sure to be at the venue at least 2 day moments before the concert so that you can set up the merch stand and do a sound check."
          "You can order merchandise from the Merchandise Workshop to sell at your concerts. They take a week to deliver, so make sure you order them in time!" ]

    lineBreak ()

    showContinuationPrompt ()
