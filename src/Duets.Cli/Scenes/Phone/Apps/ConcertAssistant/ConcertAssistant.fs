module Duets.Cli.Scenes.Phone.Apps.ConcertAssistant.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

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
