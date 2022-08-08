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
        Rehearsal.hireMemberCharacterDescription
            availableMember.Character.Name
            availableMember.Character.Gender
        |> showMessage

        availableMember.Skills
        |> List.map (fun (skill, level) -> (level, Generic.skillName skill.Id))
        |> showBarChart

        let hired =
            showConfirmationPrompt (
                Rehearsal.hireMemberConfirmation
                    availableMember.Character.Gender
            )

        if hired then
            hireMember (State.get ()) band availableMember
            |> Cli.Effect.apply

            Rehearsal.hireMemberHired |> showMessage
        else
            let continueHiring =
                showConfirmationPrompt (
                    Rehearsal.hireMemberContinueConfirmation
                )

            if continueHiring then
                promptForMemberSelection role
            else
                ()

    let private roleText instrumentType = Generic.role instrumentType

    /// Command to hire a new member for the band.
    let get =
        { Name = "hire member"
          Description = Command.hireMemberDescription
          Handler =
            (fun _ ->
                let availableRoles = Data.Roles.all

                let selectedRole =
                    showOptionalChoicePrompt
                        Rehearsal.hireMemberRolePrompt
                        Generic.cancel
                        roleText
                        availableRoles

                match selectedRole with
                | Some role -> promptForMemberSelection role
                | None -> ()

                Scene.World) }