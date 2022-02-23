module Cli.Scenes.Management.Hire

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation.Bands.Members
open Simulation.Queries

let rec hireSubScene () =
    let availableRoles = Database.roles ()

    let selectedRole =
        showOptionalChoicePrompt
            (RehearsalSpaceText HireMemberRolePrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            I18n.constant
            availableRoles

    match selectedRole with
    | Some role -> promptForMemberSelection role
    | None -> Scene.World

and promptForMemberSelection role =
    let state = State.get ()

    let band = Bands.currentBand state

    let instrument = Instrument.createInstrument (Instrument.Type.from role)

    let availableMember =
        membersForHire state band instrument.Type
        |> Seq.head

    showMemberForHire band role availableMember

and showMemberForHire band selectedInstrument availableMember =
    HireMemberCharacterDescription(
        availableMember.Character.Name,
        availableMember.Character.Gender
    )
    |> RehearsalSpaceText
    |> I18n.translate
    |> showMessage

    availableMember.Skills
    |> List.map (fun (skill, level) ->
        (level, I18n.translate (CommonText(CommonSkillName skill.Id))))
    |> showBarChart

    let hired =
        showConfirmationPrompt (
            HireMemberConfirmation availableMember.Character.Gender
            |> RehearsalSpaceText
            |> I18n.translate
        )

    if hired then
        hireMember (State.get ()) band availableMember
        |> Effect.apply

        RehearsalSpaceText HireMemberHired
        |> I18n.translate
        |> showMessage

        Scene.Management
    else
        let continueHiring =
            showConfirmationPrompt (
                RehearsalSpaceText HireMemberContinueConfirmation
                |> I18n.translate
            )

        if continueHiring then
            hireSubScene ()
        else
            Scene.Management
