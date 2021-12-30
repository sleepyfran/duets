module Cli.View.Scenes.Management.Hire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation.Bands.Members
open Simulation.Queries

let rec hireSubScene () =
    seq {
        yield
            Prompt
                { Title =
                      I18n.translate (RehearsalSpaceText HireMemberRolePrompt)
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = instrumentOptions
                            Handler =
                                basicOptionalChoiceHandler (Scene Management)
                                <| memberSelection
                            BackText = I18n.translate (CommonText CommonCancel) } }
    }

and memberSelection selectedInstrument =
    let state = State.Root.get ()

    let band = Bands.currentBand state

    let instrument =
        Instrument.createInstrument (Instrument.Type.from selectedInstrument.Id)

    membersForHire state band instrument.Type
    |> Seq.take 1
    |> Seq.map (showMemberForHire band selectedInstrument)
    |> Seq.concat

and showMemberForHire band selectedInstrument availableMember =
    seq {
        yield
            HireMemberSkillSummary(
                availableMember.Character.Name,
                availableMember.Character.Gender
            )
            |> RehearsalSpaceText
            |> I18n.translate
            |> Message

        yield!
            availableMember.Skills
            |> Seq.map
                (fun (skill, level) ->
                    HireMemberSkillLine(skill.Id, level)
                    |> RehearsalSpaceText
                    |> I18n.translate
                    |> Message)

        yield
            Prompt
                { Title =
                      HireMemberConfirmation availableMember.Character.Gender
                      |> RehearsalSpaceText
                      |> I18n.translate
                  Content =
                      ConfirmationPrompt
                      <| handleHiringConfirmation
                          band
                          selectedInstrument
                          availableMember }
    }

and handleHiringConfirmation band selectedInstrument memberForHire confirmed =
    let state = State.Root.get ()

    seq {
        if confirmed then
            yield Effect <| hireMember state band memberForHire

            yield
                Message
                <| I18n.translate (RehearsalSpaceText HireMemberHired)

            yield Scene Management
        else
            yield
                Prompt
                    { Title =
                          I18n.translate (
                              RehearsalSpaceText HireMemberContinueConfirmation
                          )
                      Content =
                          ConfirmationPrompt
                          <| handleContinueConfirmation selectedInstrument }
    }

and handleContinueConfirmation selectedInstrument confirmed =
    seq {
        if confirmed then
            yield! memberSelection selectedInstrument
        else
            yield Scene Management
    }
