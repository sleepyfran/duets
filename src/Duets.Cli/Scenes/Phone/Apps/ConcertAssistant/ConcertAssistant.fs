module Duets.Cli.Scenes.Phone.Apps.ConcertAssistant.Root

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

type private ConcertMenuOption =
    | ScheduleSoloShow
    | ScheduleOpeningActShow

let private textFromOption opt =
    match opt with
    | ScheduleSoloShow -> "Schedule a show with your band as the headliner"
    | ScheduleOpeningActShow -> "Scheduled a show supporting another band"

let rec concertAssistantApp () =
    let selectedChoice =
        showOptionalChoicePrompt
            Phone.concertAssistantAppPrompt
            Generic.nothing
            textFromOption
            [ ScheduleSoloShow; ScheduleOpeningActShow ]

    match selectedChoice with
    | Some ScheduleSoloShow -> SoloShow.scheduleShow concertAssistantApp
    | Some ScheduleOpeningActShow -> OpeningAct.scheduleShow concertAssistantApp
    | None -> Scene.Phone
