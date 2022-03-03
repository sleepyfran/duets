module Cli.Scenes.Management.Fire

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation.Queries
open Simulation.Bands.Members

let private textFromMember (bandMember: CurrentMember) =
    FireMemberListItem(bandMember.Character.Name, bandMember.Role)
    |> RehearsalSpaceText
    |> I18n.translate

let rec fireSubScene () =
    let state = State.get ()

    let bandMembers =
        Bands.currentBandMembersWithoutPlayableCharacter state

    if List.isEmpty bandMembers then
        RehearsalSpaceText FireMemberNoMembersToFire
        |> I18n.translate
        |> showMessage

        Scene.Management
    else
        promptForMember bandMembers

and promptForMember bandMembers =
    let selectedMember =
        showOptionalChoicePrompt
            (RehearsalSpaceText FireMemberPrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            textFromMember
            bandMembers

    match selectedMember with
    | Some bandMember -> promptForConfirmFiring bandMember
    | None -> Scene.Management

and promptForConfirmFiring bandMember =
    let confirmed =
        showConfirmationPrompt (
            FireMemberConfirmation bandMember.Character.Name
            |> RehearsalSpaceText
            |> I18n.translate
        )

    if confirmed then
        let state = State.get ()
        let currentBand = Bands.currentBand state

        fireMember state currentBand bandMember
        |> Result.unwrap
        |> Effect.apply

    Scene.Management
