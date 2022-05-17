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
          Description =
            I18n.translate (CommandText CommandListMembersDescription)
          Handler =
            (fun _ ->
                RehearsalSpaceText MemberListCurrentTitle
                |> I18n.translate
                |> showMessage

                currentMembers
                |> List.iter (fun cm ->
                    let character =
                        Queries.Characters.find (State.get ()) cm.CharacterId

                    MemberListCurrentMember(character.Name, cm.Role, cm.Since)
                    |> RehearsalSpaceText
                    |> I18n.translate
                    |> showMessage)

                if not (List.isEmpty pastMembers) then
                    RehearsalSpaceText MemberListPastTitle
                    |> I18n.translate
                    |> showMessage

                    pastMembers
                    |> List.iter (fun pm ->
                        let character =
                            Queries.Characters.find
                                (State.get ())
                                pm.CharacterId

                        MemberListPastMember(
                            character.Name,
                            pm.Role,
                            fst pm.Period,
                            snd pm.Period
                        )
                        |> RehearsalSpaceText
                        |> I18n.translate
                        |> showMessage)

                Scene.World) }
