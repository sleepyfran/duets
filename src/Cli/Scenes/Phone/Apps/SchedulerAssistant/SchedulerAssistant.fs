module Cli.Scenes.Phone.Apps.SchedulerAssistant.Root

open Cli.Components
open Cli.SceneIndex
open Cli.Text

type private SchedulerMenuOption =
    | ScheduleShow
    | Agenda

let private textFromOption opt =
    match opt with
    | ScheduleShow -> Phone.schedulerAssistantAppShow
    | Agenda -> Phone.schedulerAssistantAppAgenda

let rec schedulerAssistantApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppPrompt
            Generic.nothing
            textFromOption
            [ ScheduleShow; Agenda ]

    match selectedChoice with
    | Some ScheduleShow -> Show.scheduleShow schedulerAssistantApp
    | Some Agenda -> Agenda.showAgenda schedulerAssistantApp
    | None -> Scene.Phone
