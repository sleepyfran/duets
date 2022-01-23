module Cli.View.Scenes.Phone.Apps.SchedulerAssistant.Root

open Cli.View.Actions
open Cli.View.Text
open Cli.View.Common

let private schedulerAssistantOptions =
    [ { Id = "show"
        Text = I18n.translate (PhoneText SchedulerAssistantAppShow) } ]

let rec schedulerAssistantApp () =
    seq {
        yield
            Prompt
                { Title = I18n.translate (PhoneText SchedulerAssistantAppPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = schedulerAssistantOptions
                            Handler =
                                phoneOptionalChoiceHandler processSelection
                            BackText = I18n.translate (CommonText CommonNothing) } }
    }

and private processSelection choice =
    seq {
        match choice.Id with
        | "show" -> yield! Show.scheduleShow schedulerAssistantApp
        | _ -> yield NoOp
    }
