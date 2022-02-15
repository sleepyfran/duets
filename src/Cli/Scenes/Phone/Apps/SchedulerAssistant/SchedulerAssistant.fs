module Cli.Scenes.Phone.Apps.SchedulerAssistant.Root

open Cli.Components
open Cli.SceneIndex
open Cli.Text

type private SchedulerMenuOption =
    | ScheduleShow
    | Agenda

let private textFromOption opt =
    match opt with
    | ScheduleShow -> PhoneText SchedulerAssistantAppShow
    | Agenda -> PhoneText SchedulerAssistantAppAgenda
    |> I18n.translate

let rec schedulerAssistantApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppPrompt
             |> I18n.translate)
            (CommonText CommonNothing |> I18n.translate)
            textFromOption
            [ ScheduleShow; Agenda ]

    match selectedChoice with
    | Some ScheduleShow -> Show.scheduleShow schedulerAssistantApp
    | Some Agenda -> Agenda.showAgenda schedulerAssistantApp
    | None -> Scene.Phone
