module Cli.View.Scenes.BandCreator

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Simulation.Setup
open Entities

let rec bandCreator (character: Character) =
    seq {
        yield
            Prompt
                { Title = TextConstant BandCreatorInitialPrompt
                  Content = TextPrompt <| genrePrompt character }
    }

and genrePrompt character name =
    seq {
        yield
            Prompt
                { Title = TextConstant BandCreatorGenrePrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = genreOptions
                            Handler = instrumentPrompt character name } }
    }

and instrumentPrompt character name genre =
    seq {
        yield
            Prompt
                { Title = TextConstant BandCreatorInstrumentPrompt
                  Content =
                      ChoicePrompt
                      <| MandatoryChoiceHandler
                          { Choices = instrumentOptions
                            Handler = confirmationPrompt character name genre.Id } }
    }

and confirmationPrompt character name genre role =
    seq {
        yield
            Prompt
                { Title =
                      TextConstant
                      <| BandCreatorConfirmationPrompt(
                          character.Name,
                          name,
                          genre,
                          role.Id
                      )
                  Content =
                      ConfirmationPrompt
                      <| handleConfirmation character name genre role }
    }

and handleConfirmation character name genre role confirmed =
    seq {
        if confirmed then
            let members =
                [ Band.Member.from
                      character
                      (Instrument.Type.from role.Id)
                      (Calendar.fromDayMonth 1 1) ]

            match (Band.from name genre members) with
            | Ok band ->
                yield Effect <| startGame character band
                yield Scene RehearsalRoom
            | Error Band.NameTooShort ->
                yield!
                    seq {
                        Message
                        <| TextConstant CreatorErrorBandNameTooShort

                        yield! bandCreator character
                    }
            | Error Band.NameTooLong ->
                yield!
                    seq {
                        Message
                        <| TextConstant CreatorErrorBandNameTooLong

                        yield! bandCreator character
                    }
            | Error Band.NoMembersGiven -> yield! [] // Dead end, should never happen.
        else
            yield Scene CharacterCreator
    }
