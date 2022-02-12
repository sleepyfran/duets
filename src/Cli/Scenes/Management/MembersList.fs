module Cli.Scenes.Management.MemberList

open Agents
open Cli.Actions
open Cli.Text
open Simulation.Queries

/// Shows the current and past members of the band.
let rec memberListSubScene () =
    let state = State.get ()
    let currentMembers = Bands.currentBandMembers state
    let pastMembers = Bands.pastBandMembers state

    seq {
        yield
            Message
            <| I18n.translate (RehearsalSpaceText MemberListCurrentTitle)

        yield!
            currentMembers
            |> List.map
                (fun m ->
                    MemberListCurrentMember(m.Character.Name, m.Role, m.Since)
                    |> RehearsalSpaceText
                    |> I18n.translate
                    |> Message)

        if not (List.isEmpty pastMembers) then
            yield
                Message
                <| I18n.translate (RehearsalSpaceText MemberListPastTitle)

            yield!
                pastMembers
                |> List.map
                    (fun m ->
                        MemberListPastMember(
                            m.Character.Name,
                            m.Role,
                            fst m.Period,
                            snd m.Period
                        )
                        |> RehearsalSpaceText
                        |> I18n.translate
                        |> Message)

        yield Scene Management
    }
