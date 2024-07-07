module Duets.Cli.Scenes.Phone.Apps.Flights.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

type private FlightMenuOption = | BookFlight

let private textFromOption opt =
    match opt with
    | BookFlight -> Phone.bookFlightOption

let rec private displayUpcomingFlights flights =
    let tableColumns =
        [ Phone.flightsOriginHeader
          Phone.flightsDestinationHeader
          Phone.flightsDateHeader ]

    let tableRows =
        flights
        |> List.map (fun flight ->
            [ Phone.flightsCityRow flight.Origin
              Phone.flightsCityRow flight.Destination
              Phone.flightsDateRow flight.Date flight.DayMoment ])

    showTable tableColumns tableRows

let rec flightsApp () =
    let _, upcomingFlights = Queries.Flights.all (State.get ())

    if List.isEmpty upcomingFlights then
        Phone.flightsNoUpcomingWelcome |> showMessage
    else
        displayUpcomingFlights upcomingFlights

    let option =
        showOptionalChoicePrompt
            Phone.flightsAppPrompt
            Generic.back
            textFromOption
            [ BookFlight ]

    match option with
    | Some BookFlight -> BookFlight.bookFlight flightsApp
    | _ -> Scene.Phone
