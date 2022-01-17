module Cli.View.Scenes.Management.Fire

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Common
open Entities
open Simulation.Queries
open Simulation.Bands.Members

let rec fireSubScene () =
    let state = State.get ()

    let memberOptions =
        Bands.currentBandMembersWithoutPlayableCharacter state
        |> List.map
            (fun m ->
                let (CharacterId (id)) = m.Character.Id

                { Id = id.ToString()
                  Text =
                      FireMemberListItem(m.Character.Name, m.Role)
                      |> RehearsalSpaceText
                      |> I18n.translate })

    seq {
        if memberOptions.Length = 0 then
            yield
                Message
                <| I18n.translate (RehearsalSpaceText FireMemberNoMembersToFire)

            yield Scene Management
        else
            yield
                Prompt
                    { Title =
                          I18n.translate (RehearsalSpaceText FireMemberPrompt)
                      Content =
                          ChoicePrompt
                          <| OptionalChoiceHandler
                              { Choices = memberOptions
                                Handler =
                                    basicOptionalChoiceHandler (
                                        Scene Management
                                    )
                                    <| confirmFiring
                                BackText =
                                    I18n.translate (CommonText CommonCancel) } }
    }

and confirmFiring selectedMember =
    let state = State.get ()

    let band = Bands.currentBand state
    let memberToFire = memberFromSelection band selectedMember

    seq {
        yield
            Prompt
                { Title =
                      FireMemberConfirmation memberToFire.Character.Name
                      |> RehearsalSpaceText
                      |> I18n.translate
                  Content =
                      ConfirmationPrompt(handleConfirmation band memberToFire) }
    }

and handleConfirmation band memberToFire confirmed =
    let state = State.get ()

    seq {
        if confirmed then
            yield
                fireMember state band memberToFire
                |> Result.unwrap
                |> Effect

            yield
                FireMemberConfirmed memberToFire.Character.Name
                |> RehearsalSpaceText
                |> I18n.translate
                |> Message

            yield Scene Management
        else
            yield Scene Management
    }
