module Duets.Cli.Scenes.Phone.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

type private PhoneMenuOption =
    | Bank
    | BnB
    | Calendar
    | ConcertAssistant
    | Duber
    | Flights
    | FoodDelivery
    | Jobs
    | Mastodon
    | Statistics
    | Weather

let private textFromOption opt =
    match opt with
    | Bank -> "Bank"
    | BnB -> "DuetsBnB"
    | Calendar -> "Calendar"
    | ConcertAssistant -> "Concert Assistant"
    | Duber -> "Duber Taxi"
    | Flights -> "Flights"
    | FoodDelivery -> "Duelivery"
    | Jobs -> "Jobs"
    | Mastodon -> "Mastodon"
    | Statistics -> "Statistics"
    | Weather -> "Weather"

let private availableApps =
    [ Bank
      BnB
      Calendar
      ConcertAssistant
      Duber
      Flights
      FoodDelivery
      Jobs
      Mastodon
      Statistics
      Weather ]

let rec phoneScene () =
    let currentDate = State.get () |> Queries.Calendar.today

    let dayMoment = Calendar.Query.dayMomentOf currentDate

    let selection =
        showOptionalChoicePrompt
            (Phone.prompt currentDate dayMoment)
            Phone.putDown
            textFromOption
            availableApps

    match selection with
    | Some Bank -> Apps.Bank.Root.bankApp ()
    | Some BnB -> Apps.BnB.Root.bnbApp ()
    | Some Calendar -> Apps.Calendar.Root.calendarApp ()
    | Some ConcertAssistant -> Apps.ConcertAssistant.Root.concertAssistantApp ()
    | Some Duber -> Apps.Duber.Root.duberApp ()
    | Some Flights -> Apps.Flights.Root.flightsApp ()
    | Some FoodDelivery -> Apps.FoodDelivery.foodDeliveryApp ()
    | Some Jobs -> Apps.Jobs.Root.jobsApp ()
    | Some Mastodon -> Apps.Mastodon.Root.mastodonApp ()
    | Some Statistics -> Apps.Statistics.Root.statisticsApp ()
    | Some Weather -> Apps.Weather.Root.weatherApp ()
    | None -> Scene.World
