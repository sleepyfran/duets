module Cli.Scenes.Phone.Apps.ConcertAssistant.Root

open Cli.Components
open Cli.SceneIndex
open Cli.Text

type private ConcertMenuOption = | ScheduleShow

let private textFromOption opt =
    match opt with
    | ScheduleShow -> Phone.concertAssistantAppShow

let rec concertAssistantApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.concertAssistantAppPrompt
            Generic.nothing
            textFromOption
            [ ScheduleShow ]

    match selectedChoice with
    | Some ScheduleShow -> Show.scheduleShow concertAssistantApp
    | None -> Scene.Phone
