module Cli.View.Scenes.Management.Root

open Cli.View.Actions
open Cli.View.TextConstants
open Cli.View.Scenes.Management.Hire

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
          Content = ChoicePrompt(managementOptions, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "hire" -> yield! hireScene ()
    | "fire" -> yield NoOp
    | _ -> yield NoOp
  }
