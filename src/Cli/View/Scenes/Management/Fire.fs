module Cli.View.Scenes.Management.Fire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Queries
open Simulation.Bands.Members

let rec fireScene () =
  let memberOptions =
    currentBandMembersWithoutPlayableCharacter ()
    |> List.map
         (fun m ->
           let (CharacterId (id)) = m.Character.Id

           { Id = id.ToString()
             Text =
               TextConstant
               <| FireMemberListItem(m.Character.Name, m.Role) })

  seq {
    if memberOptions.Length = 0 then
      yield Message <| TextConstant FireMemberNoMembersToFire
      yield Scene RehearsalRoom
    else
      yield
        Prompt
          { Title = TextConstant FireMemberPrompt
            Content =
              ChoicePrompt
              <| OptionalChoiceHandler
                   { Choices = memberOptions
                     Handler = rehearsalRoomOptionalChoiceHandler confirmFiring
                     BackText = TextConstant CommonCancel } }
  }

and confirmFiring selectedMember =
  let band = currentBand ()
  let memberToFire = memberFromSelection band selectedMember

  seq {
    yield
      Prompt
        { Title =
            TextConstant
            <| FireMemberConfirmation memberToFire.Character.Name
          Content = ConfirmationPrompt(handleConfirmation band memberToFire) }
  }

and handleConfirmation band memberToFire confirmed =
  seq {
    if confirmed then
      fireMember band memberToFire |> ignore

      yield
        FireMemberConfirmed memberToFire.Character.Name
        |> TextConstant
        |> Message

      yield Scene RehearsalRoom
    else
      yield Scene RehearsalRoom
  }
