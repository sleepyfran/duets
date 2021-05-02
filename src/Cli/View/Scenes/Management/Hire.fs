module Cli.View.Scenes.Management.Hire

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open Simulation.Bands.Members
open Simulation.Bands.Queries

let rec hireScene () =
  seq {
    yield
      Prompt
        { Title = TextConstant HireMemberRolePrompt
          Content = ChoicePrompt(instrumentOptions, memberSelection) }
  }

and memberSelection selectedInstrument =
  let band = currentBand ()

  let instrument =
    Instrument.createInstrument (Instrument.Type.from selectedInstrument.Id)

  membersForHire band instrument
  |> Seq.take 1
  |> Seq.map (showMemberForHire selectedInstrument)
  |> Seq.concat

and showMemberForHire selectedInstrument availableMember =
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
            <| handleConfirmation selectedInstrument availableMember }
  }

and handleConfirmation selectedInstrument memberForHire confirmed =
  seq {
    if confirmed then
      hireMember memberForHire
      yield Message <| TextConstant HireMemberHired
    else
      yield! memberSelection selectedInstrument
      yield Scene RehearsalRoom
  }
