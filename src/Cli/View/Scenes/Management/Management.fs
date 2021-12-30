module Cli.View.Scenes.Management.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text

let managementOptions =
    [ { Id = "hire"
        Text = I18n.translate (RehearsalSpaceText ManagementHireMember) }
      { Id = "fire"
        Text = I18n.translate (RehearsalSpaceText ManagementFireMember) }
      { Id = "members"
        Text = I18n.translate (RehearsalSpaceText ManagementMemberList) } ]

/// Creates the management menu which allows to hire and fire members.
let rec managementScene () =
    seq {
        yield
            Prompt
                { Title = I18n.translate (RehearsalSpaceText ManagementPrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = managementOptions
                            Handler =
                                worldOptionalChoiceHandler (processSelection)
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and processSelection choice =
    seq {
        match choice.Id with
        | "hire" -> yield! Hire.hireSubScene ()
        | "fire" -> yield! Fire.fireSubScene ()
        | "members" -> yield! MemberList.memberListSubScene ()
        | _ -> yield NoOp
    }
