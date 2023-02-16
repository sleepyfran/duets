module Duets.Cli.Scenes.Phone.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

type private PhoneMenuOption =
    | Bank
    | Flights
    | Jobs
    | Calendar
    | Statistics
    | ConcertAssistant

let private textFromOption opt =
    match opt with
    | Bank -> "Bank"
    | Flights -> "Flights" 
    | Jobs -> "Jobs"
    | Calendar -> "Calendar"
    | Statistics -> "Statistics" 
    | ConcertAssistant -> "Concert Assistant"

let rec phoneScene () =
    let currentDate = State.get () |> Queries.Calendar.today

    let dayMoment = Calendar.Query.dayMomentOf currentDate

    let selection =
        showOptionalChoicePrompt
            (Phone.prompt currentDate dayMoment)
            Phone.putDown
            textFromOption
            [ Bank; Flights; Jobs; Calendar; Statistics; ConcertAssistant ]

    match selection with
    | Some Bank -> Apps.Bank.Root.bankApp ()
    | Some Flights -> Apps.Flights.Root.flightsApp ()
    | Some Jobs -> Apps.Jobs.Root.jobsApp ()
    | Some Calendar -> Apps.Calendar.Root.calendarApp ()
    | Some Statistics -> Apps.Statistics.Root.statisticsApp ()
    | Some ConcertAssistant -> Apps.ConcertAssistant.Root.concertAssistantApp ()
    | None -> Scene.World
