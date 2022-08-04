namespace Cli.Components.Commands

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities
open Simulation

[<RequireQualifiedAccess>]
module ListMembersCommand =
    /// Command to discard an unfinished song.
    let create (currentMembers: CurrentMember list) pastMembers =
        { Name = "list members"
          Description = Command.listMembersDescription
          Handler =
            (fun _ ->
                let currentMemberColumns =
                    [ Rehearsal.memberListNameHeader
                      Rehearsal.memberListRoleHeader
                      Rehearsal.memberListSinceHeader ]

                let currentMemberRows =
                    currentMembers
                    |> List.map (fun cm ->
                        let character =
                            Queries.Characters.find
                                (State.get ())
                                cm.CharacterId

                        [ Rehearsal.memberListName character.Name
                          Rehearsal.memberListRole cm.Role
                          Rehearsal.memberListSince cm.Since ])

                showTableWithTitle
                    Rehearsal.memberListCurrentTitle
                    currentMemberColumns
                    currentMemberRows

                if not (List.isEmpty pastMembers) then
                    let pastMemberColumns =
                        currentMemberColumns
                        @ [ Rehearsal.memberListUntilHeader ]

                    let pastMemberRows =
                        pastMembers
                        |> List.map (fun pm ->
                            let character =
                                Queries.Characters.find
                                    (State.get ())
                                    pm.CharacterId

                            [ Rehearsal.memberListName character.Name
                              Rehearsal.memberListRole pm.Role
                              Rehearsal.memberListSince (fst pm.Period)
                              Rehearsal.memberListUntil (snd pm.Period) ])

                    showTableWithTitle
                        Rehearsal.memberListPastTitle
                        pastMemberColumns
                        pastMemberRows

                Scene.World) }
