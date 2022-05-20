namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation
open Simulation.Bands.Members

[<RequireQualifiedAccess>]
module HireMemberCommand =
    let rec private promptForMemberSelection role =
        let state = State.get ()

        let band = Queries.Bands.currentBand state

        let instrument =
            Instrument.createInstrument role

        let availableMember =
            membersForHire state band instrument.Type
            |> Seq.head

        showMemberForHire role band availableMember

    and private showMemberForHire role band availableMember =
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
            |> Cli.Effect.apply

            RehearsalSpaceText HireMemberHired
            |> I18n.translate
            |> showMessage
        else
            let continueHiring =
                showConfirmationPrompt (
                    RehearsalSpaceText HireMemberContinueConfirmation
                    |> I18n.translate
                )

            if continueHiring then
                promptForMemberSelection role
            else
                ()

    let private roleText instrumentType =
        CommonRole instrumentType
        |> CommonText
        |> I18n.translate

    /// Command to hire a new member for the band.
    let get =
        { Name = "hire member"
          Description =
            I18n.translate (CommandText CommandHireMemberDescription)
          Handler =
            (fun _ ->
                let availableRoles = Database.roles

                let selectedRole =
                    showOptionalChoicePrompt
                        (RehearsalSpaceText HireMemberRolePrompt
                         |> I18n.translate)
                        (CommonText CommonCancel |> I18n.translate)
                        roleText
                        availableRoles

                match selectedRole with
                | Some role -> promptForMemberSelection role
                | None -> ()

                Scene.World) }
