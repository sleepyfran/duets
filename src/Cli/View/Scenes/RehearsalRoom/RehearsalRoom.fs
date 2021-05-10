module Cli.View.Scenes.RehearsalRoom.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Cli.View.Scenes.RehearsalRoom.Compose

let rehearsalOptions =
    [ { Id = "compose"
        Text = TextConstant RehearsalRoomCompose }
      { Id = "manage"
        Text = TextConstant RehearsalRoomManage } ]

/// Creates the rehearsal room which allows to access the compose and managing
/// section.
let rec rehearsalRoomScene () =
    seq {
        yield Figlet <| TextConstant RehearsalRoomTitle

        yield
            Message
            <| TextConstant(CommonYouAreIn "the rehearsal room")

        yield
            Prompt
                { Title = TextConstant RehearsalRoomPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = rehearsalOptions
                            Handler =
                                mainMenuOptionalChoiceHandler processSelection
                            BackText = TextConstant CommonBackToMainMenu } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "compose" -> yield SubScene SubScene.RehearsalRoomCompose
        | "manage" -> yield Scene Management
        | _ -> yield NoOp
    }
