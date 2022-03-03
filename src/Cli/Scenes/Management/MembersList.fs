module Cli.Scenes.Management.MemberList

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Simulation.Queries

/// Shows the current and past members of the band.
let rec memberListSubScene () =
    let state = State.get ()
    let currentMembers = Bands.currentBandMembers state
    let pastMembers = Bands.pastBandMembers state

    RehearsalSpaceText MemberListCurrentTitle
    |> I18n.translate
    |> showMessage

    currentMembers
    |> List.iter
        (fun cm ->
            MemberListCurrentMember(cm.Character.Name, cm.Role, cm.Since)
            |> RehearsalSpaceText
            |> I18n.translate
            |> showMessage)

    if not (List.isEmpty pastMembers) then
        RehearsalSpaceText MemberListPastTitle
        |> I18n.translate
        |> showMessage

        pastMembers
        |> List.iter
            (fun pm ->
                MemberListPastMember(
                    pm.Character.Name,
                    pm.Role,
                    fst pm.Period,
                    snd pm.Period
                )
                |> RehearsalSpaceText
                |> I18n.translate
                |> showMessage)

    Scene.Management
