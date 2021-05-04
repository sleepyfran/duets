module Cli.View.Scenes.Management.MemberList

open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Bands.Queries

/// Shows the current and past members of the band.
let rec memberListScene () =
  let currentMembers = currentBandMembers ()
  let pastMembers = pastBandMembers ()

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

    yield SceneAfterKey Management
  }
