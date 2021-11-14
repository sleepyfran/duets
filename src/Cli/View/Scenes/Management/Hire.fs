module Cli.View.Scenes.Management.Hire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Members
open Simulation.Queries

let rec hireScene state =
    seq {
        yield
            Prompt
                { Title = TextConstant HireMemberRolePrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = instrumentOptions
                            Handler =
                                basicOptionalChoiceHandler (Scene Management)
                                <| memberSelection state
                            BackText = TextConstant CommonCancel } }
    }

and memberSelection state selectedInstrument =
    let band = Bands.currentBand state

    let instrument =
        Instrument.createInstrument (Instrument.Type.from selectedInstrument.Id)

    membersForHire state band instrument.Type
    |> Seq.take 1
    |> Seq.map (showMemberForHire state band selectedInstrument)
    |> Seq.concat

and showMemberForHire state band selectedInstrument availableMember =
    seq {
        yield
            HireMemberSkillSummary(
                availableMember.Character.Name,
                availableMember.Character.Gender
            )
            |> TextConstant
            |> Message

        yield!
            availableMember.Skills
            |> Seq.map
                (fun (skill, level) ->
                    HireMemberSkillLine(skill.Id, level)
                    |> TextConstant
                    |> Message)

        yield
            Prompt
                { Title =
                      TextConstant
                      <| HireMemberConfirmation availableMember.Character.Gender
                  Content =
                      ConfirmationPrompt
                      <| handleHiringConfirmation
                          state
                          band
                          selectedInstrument
                          availableMember }
    }

and handleHiringConfirmation
    state
    band
    selectedInstrument
    memberForHire
    confirmed
    =
    seq {
        if confirmed then
            yield Effect <| hireMember state band memberForHire
            yield Message <| TextConstant HireMemberHired
            yield Scene Management
        else
            yield
                Prompt
                    { Title = TextConstant HireMemberContinueConfirmation
                      Content =
                          ConfirmationPrompt
                          <| handleContinueConfirmation state selectedInstrument }
    }

and handleContinueConfirmation state selectedInstrument confirmed =
    seq {
        if confirmed then
            yield! memberSelection state selectedInstrument
        else
            yield Scene Management
    }
