module Cli.Scenes.Management.Root

open Cli.Components
open Cli.SceneIndex
open Cli.Text

type private ManagementMenuOptions =
    | Hire
    | Fire
    | ListMembers

let private textFromOption opt =
    match opt with
    | Hire -> RehearsalSpaceText ManagementHireMember
    | Fire -> RehearsalSpaceText ManagementFireMember
    | ListMembers -> RehearsalSpaceText ManagementMemberList
    |> I18n.translate

/// Creates the management menu which allows to hire and fire members.
let rec managementScene () =
    let selectedChoice =
        showOptionalChoicePrompt
            (RehearsalSpaceText ManagementPrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            textFromOption
            [ Hire; Fire; ListMembers ]

    match selectedChoice with
    | Some Hire -> Hire.hireSubScene ()
    | Some Fire -> Fire.fireSubScene ()
    | Some ListMembers -> MemberList.memberListSubScene ()
    | None -> Scene.World
