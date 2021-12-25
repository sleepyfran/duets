module Cli.View.Scenes.Management.Hire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Members
open Simulation.Queries

let rec hireSubScene () =
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
                                <| memberSelection
                            BackText = TextConstant CommonCancel } }
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
                          band
                          selectedInstrument
                          availableMember }
    }

and handleHiringConfirmation band selectedInstrument memberForHire confirmed =
    let state = State.Root.get ()

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
                          <| handleContinueConfirmation selectedInstrument }
    }

and handleContinueConfirmation selectedInstrument confirmed =
    seq {
        if confirmed then
            yield! memberSelection selectedInstrument
        else
            yield Scene Management
    }
