module Cli.View.Scenes.Map

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let mapOptions =
    seq {
        yield
            { Id = "rehearsal_room"
              Text = TextConstant MapOptionRehearsalRoom }
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
        | _ -> yield NoOp
    }
