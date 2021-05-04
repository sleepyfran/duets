module Cli.View.Scenes.Management.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Cli.View.Scenes.Management.Hire
open Cli.View.Scenes.Management.Fire

let managementOptions =
  [ { Id = "hire"
      Text = TextConstant ManagementHireMember }
    { Id = "fire"
      Text = TextConstant ManagementFireMember } ]

/// Creates the management menu which allows to hire and fire members.
let rec managementScene () =
  seq {
    yield Figlet <| TextConstant ManagementTitle

    yield
      Prompt
        { Title = TextConstant ManagementPrompt
          Content =
            ChoicePrompt
            <| OptionalChoiceHandler
                 { Choices = managementOptions
                   Handler = rehearsalRoomOptionalChoiceHandler processSelection
                   BackText = TextConstant CommonCancel } }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "hire" -> yield! hireScene ()
    | "fire" -> yield! fireScene ()
    | _ -> yield NoOp
  }
