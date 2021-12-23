module Cli.View.Scenes.Management.Fire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Common
open Entities
open Simulation.Queries
open Simulation.Bands.Members

let rec fireScene space rooms =
    let state = State.Root.get ()

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
            yield Scene(Scene.RehearsalRoom(space, rooms))
        else
            yield
                Prompt
                    { Title = TextConstant FireMemberPrompt
                      Content =
                          ChoicePrompt
                          <| OptionalChoiceHandler
                              { Choices = memberOptions
                                Handler =
                                    basicOptionalChoiceHandler (
                                        Scene(Management(space, rooms))
                                    )
                                    <| confirmFiring space rooms
                                BackText = TextConstant CommonCancel } }
    }

and confirmFiring space rooms selectedMember =
    let state = State.Root.get ()

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
                          handleConfirmation space rooms band memberToFire
                      ) }
    }

and handleConfirmation space rooms band memberToFire confirmed =
    let state = State.Root.get ()

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

            yield Scene(Management(space, rooms))
        else
            yield Scene(Management(space, rooms))
    }
