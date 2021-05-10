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
        yield Figlet <| TextConstant ManagementTitle

        yield
            Prompt
                { Title = TextConstant ManagementPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = managementOptions
                            Handler =
                                rehearsalRoomOptionalChoiceHandler
                                    processSelection
                            BackText = TextConstant CommonCancel } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "hire" -> yield SubScene SubScene.ManagementHireMember
        | "fire" -> yield SubScene SubScene.ManagementFireMember
        | "members" -> yield SubScene SubScene.ManagementListMembers
        | _ -> yield NoOp
    }
