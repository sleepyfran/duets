module Cli.View.Scenes.Map

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let mapOptions =
    seq {
        yield
            { Id = "rehearsal_room"
              Text = TextConstant MapOptionRehearsalRoom }
        yield
            { Id = "bank"
              Text = TextConstant MapOptionBank }
    }
    |> List.ofSeq

let rec mapScene () =
    seq {
        yield Figlet <| TextConstant MapTitle

        yield
            Prompt
                { Title = TextConstant MapPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = mapOptions
                            Handler =
                                mainMenuOptionalChoiceHandler processSelection
                            BackText = TextConstant CommonBackToMainMenu } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "rehearsal_room" -> yield Scene RehearsalRoom
        | "bank" -> yield Scene Bank
        | _ -> yield NoOp
    }
