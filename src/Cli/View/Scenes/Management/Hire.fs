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
          Content =
            ChoicePrompt
            <| OptionalChoiceHandler
                 { Choices = instrumentOptions
                   Handler = rehearsalRoomOptionalChoiceHandler memberSelection
                   BackText = TextConstant CommonCancel } }
  }

and memberSelection selectedInstrument =
  let band = currentBand ()

  let instrument =
    Instrument.createInstrument (Instrument.Type.from selectedInstrument.Id)

  membersForHire band instrument
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
            <| handleConfirmation band selectedInstrument availableMember }
  }

and handleConfirmation band selectedInstrument memberForHire confirmed =
  seq {
    if confirmed then
      hireMember band memberForHire
      yield Message <| TextConstant HireMemberHired
      yield Scene RehearsalRoom
    else
      yield! memberSelection selectedInstrument
  }
