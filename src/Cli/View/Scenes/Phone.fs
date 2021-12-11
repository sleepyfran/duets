module Cli.View.Scenes.Phone

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let phoneOptions =
    [ yield
        { Id = "bank"
          Text = TextConstant PhoneOptionBank }

      yield
          { Id = "statistics"
            Text = TextConstant PhoneOptionStatistics } ]

let rec phoneScene () =
    seq {
        yield
            Prompt
                { Title = TextConstant PhonePrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = phoneOptions
                            Handler =
                                worldOptionalChoiceHandler <| processSelection
                            BackText = TextConstant CommonBackToWorld } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "bank" -> yield Scene Bank
        | "statistics" -> yield Scene Statistics
        | _ -> yield NoOp
    }
