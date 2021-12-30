module Cli.View.Scenes.Phone

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text

let phoneOptions =
    [ yield
        { Id = "bank"
          Text = I18n.translate (PhoneText PhoneOptionBank) }

      yield
          { Id = "statistics"
            Text = I18n.translate (PhoneText PhoneOptionStatistics) } ]

let rec phoneScene () =
    seq {
        yield
            Prompt
                { Title = I18n.translate (PhoneText PhonePrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = phoneOptions
                            Handler =
                                worldOptionalChoiceHandler <| processSelection
                            BackText =
                                I18n.translate (CommonText CommonBackToWorld) } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "bank" -> yield Scene Bank
        | "statistics" -> yield Scene Statistics
        | _ -> yield NoOp
    }
