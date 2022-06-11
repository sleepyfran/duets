module Cli.Scenes.Phone.Root

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation

type private PhoneMenuOption =
    | Bank
    | Statistics
    | Scheduler

let private textFromOption opt =
    match opt with
    | Bank -> Phone.optionBank
    | Statistics -> Phone.optionStatistics
    | Scheduler -> Phone.optionScheduler

let rec phoneScene () =
    let currentDate =
        State.get () |> Queries.Calendar.today

    let dayMoment =
        Calendar.Query.dayMomentOf currentDate

    let selection =
        showOptionalChoicePrompt
            (Phone.prompt currentDate dayMoment)
            Generic.backToWorld
            textFromOption
            [ Bank; Statistics; Scheduler ]

    match selection with
    | Some Bank -> Apps.Bank.Root.bankApp ()
    | Some Statistics -> Apps.Statistics.Root.statisticsApp ()
    | Some Scheduler -> Apps.SchedulerAssistant.Root.schedulerAssistantApp ()
    | None -> Scene.World
