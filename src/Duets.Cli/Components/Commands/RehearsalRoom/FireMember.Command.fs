namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bands.Members

[<RequireQualifiedAccess>]
module FireMemberCommand =
    let private textFromMember (bandMember: CurrentMember) =
        let character =
            Queries.Characters.find (State.get ()) bandMember.CharacterId

        Rehearsal.fireMemberListItem (character.Name, bandMember.Role)

    let rec private promptForMember bandMembers =
        let selectedMember =
            showOptionalChoicePrompt
                Rehearsal.fireMemberPrompt
                Generic.cancel
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
                Rehearsal.fireMemberConfirmation character.Name
            )

        if confirmed then
            let state = State.get ()

            let currentBand = Queries.Bands.currentBand state

            fireMember state currentBand bandMember
            |> Result.unwrap
            |> Duets.Cli.Effect.apply
        else
            ()

    /// Command fire a member of the band.
    let create bandMembers =
        { Name = "fire member"
          Description = Command.fireMemberDescription
          Handler =
            (fun _ ->
                if List.isEmpty bandMembers then
                    Rehearsal.fireMemberNoMembersToFire |> showMessage
                else
                    promptForMember bandMembers

                Scene.World) }
