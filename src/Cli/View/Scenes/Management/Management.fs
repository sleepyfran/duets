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
let rec managementScene space rooms =
    seq {
        yield
            Prompt
                { Title = TextConstant ManagementPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = managementOptions
                            Handler =
                                rehearsalRoomOptionalChoiceHandler
                                    space
                                    rooms
                                    (processSelection space rooms)
                            BackText = TextConstant CommonCancel } }
    }

and processSelection space rooms choice =
    seq {
        match choice.Id with
        | "hire" -> yield! Hire.hireSubScene space rooms
        | "fire" -> yield! Fire.fireSubScene space rooms
        | "members" -> yield! MemberList.memberListSubScene space rooms
        | _ -> yield NoOp
    }
