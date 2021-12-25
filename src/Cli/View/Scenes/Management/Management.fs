module Cli.View.Scenes.Management.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let managementOptions =
    [ { Id = "hire"
        Text = TextConstant ManagementHireMember }
      { Id = "fire"
        Text = TextConstant ManagementFireMember }
      { Id = "members"
        Text = TextConstant ManagementMemberList } ]

/// Creates the management menu which allows to hire and fire members.
let rec managementScene () =
    seq {
        yield
            Prompt
                { Title = TextConstant ManagementPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = managementOptions
                            Handler =
                                worldOptionalChoiceHandler (processSelection)
                            BackText = TextConstant CommonCancel } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "hire" -> yield! Hire.hireSubScene ()
        | "fire" -> yield! Fire.fireSubScene ()
        | "members" -> yield! MemberList.memberListSubScene ()
        | _ -> yield NoOp
    }
