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
                Rehearsal.memberListCurrentTitle |> showMessage

                currentMembers
                |> List.iter (fun cm ->
                    let character =
                        Queries.Characters.find (State.get ()) cm.CharacterId

                    Rehearsal.memberListCurrentMember
                        character.Name
                        cm.Role
                        cm.Since
                    |> showMessage)

                if not (List.isEmpty pastMembers) then
                    Rehearsal.memberListPastTitle |> showMessage

                    pastMembers
                    |> List.iter (fun pm ->
                        let character =
                            Queries.Characters.find
                                (State.get ())
                                pm.CharacterId

                        Rehearsal.memberListPastMember
                            character.Name
                            pm.Role
                            (fst pm.Period)
                            (snd pm.Period)
                        |> showMessage)

                Scene.World) }
