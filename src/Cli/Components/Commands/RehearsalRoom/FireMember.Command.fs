namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Bands.Members

[<RequireQualifiedAccess>]
module FireMemberCommand =
    let private textFromMember (bandMember: CurrentMember) =
        let character =
            Queries.Characters.find (State.get ()) bandMember.CharacterId

        FireMemberListItem(character.Name, bandMember.Role)
        |> RehearsalSpaceText
        |> I18n.translate

    let rec private promptForMember bandMembers =
        let selectedMember =
            showOptionalChoicePrompt
                (RehearsalSpaceText FireMemberPrompt
                 |> I18n.translate)
                (CommonText CommonCancel |> I18n.translate)
                textFromMember
                bandMembers

        match selectedMember with
        | Some bandMember -> promptForConfirmFiring bandMember
        | None -> ()

    and private promptForConfirmFiring bandMember =
        let character =
            Queries.Characters.find (State.get ()) bandMember.CharacterId

        let confirmed =
            showConfirmationPrompt (
                FireMemberConfirmation character.Name
                |> RehearsalSpaceText
                |> I18n.translate
            )

        if confirmed then
            let state = State.get ()

            let currentBand =
                Queries.Bands.currentBand state

            fireMember state currentBand bandMember
            |> Result.unwrap
            |> Cli.Effect.apply
        else
            ()

    /// Command fire a member of the band.
    let create bandMembers =
        { Name = "fire member"
          Description =
            I18n.translate (CommandText CommandFireMemberDescription)
          Handler =
            (fun _ ->
                if List.isEmpty bandMembers then
                    RehearsalSpaceText FireMemberNoMembersToFire
                    |> I18n.translate
                    |> showMessage
                else
                    promptForMember bandMembers

                Scene.World) }
