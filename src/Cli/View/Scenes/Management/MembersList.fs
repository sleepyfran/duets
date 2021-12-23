module Cli.View.Scenes.Management.MemberList

open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Queries

/// Shows the current and past members of the band.
let rec memberListScene space rooms =
    let state = State.Root.get ()
    let currentMembers = Bands.currentBandMembers state
    let pastMembers = Bands.pastBandMembers state

    seq {
        yield Message <| TextConstant MemberListCurrentTitle

        yield!
            currentMembers
            |> List.map
                (fun m ->
                    MemberListCurrentMember(m.Character.Name, m.Role, m.Since)
                    |> TextConstant
                    |> Message)

        if not (List.isEmpty pastMembers) then
            yield Message <| TextConstant MemberListPastTitle

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
                        |> TextConstant
                        |> Message)

        yield Scene(Management(space, rooms))
    }
