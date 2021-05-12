module Cli.View.Scenes.Management.Fire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Common
open Entities
open Simulation.Queries
open Simulation.Bands.Members

let rec fireScene state =
    let memberOptions =
        Bands.currentBandMembersWithoutPlayableCharacter state
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
            yield SceneAfterKey RehearsalRoom
        else
            yield
                Prompt
                    { Title = TextConstant FireMemberPrompt
                      Content =
                          ChoicePrompt
                          <| OptionalChoiceHandler
                              { Choices = memberOptions
                                Handler =
                                    rehearsalRoomOptionalChoiceHandler
                                    <| confirmFiring state
                                BackText = TextConstant CommonCancel } }
    }

and confirmFiring state selectedMember =
    let band = Bands.currentBand state
    let memberToFire = memberFromSelection band selectedMember

    seq {
        yield
            Prompt
                { Title =
                      TextConstant
                      <| FireMemberConfirmation memberToFire.Character.Name
                  Content =
                      ConfirmationPrompt(
                          handleConfirmation state band memberToFire
                      ) }
    }

and handleConfirmation state band memberToFire confirmed =
    seq {
        if confirmed then
            yield
                fireMember state band memberToFire
                |> Result.unwrap
                |> Effect

            yield
                FireMemberConfirmed memberToFire.Character.Name
                |> TextConstant
                |> Message

            yield SceneAfterKey RehearsalRoom
        else
            yield Scene RehearsalRoom
    }
