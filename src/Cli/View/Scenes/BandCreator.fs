module Cli.View.Scenes.BandCreator

open Mediator.Query
open Mediator.Mutation
open Mediator.Queries.Storage
open Mediator.Mutations.Setup
open Cli.View.Actions
open Cli.View.TextConstants

let rec bandCreator (character: CharacterInput) =
  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInitialPrompt
          Content = TextPrompt <| handleName character }
  }

and handleName character name =
  let genreOptions =
    query GenresQuery
    |> List.map (fun genre -> { Id = genre; Text = Literal genre })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorGenrePrompt
          Content = ChoicePrompt(genreOptions, handleGenre character name) }
  }

and handleGenre character name genre =
  let roleOptions =
    query RolesQuery
    |> List.map (fun role -> { Id = role; Text = Literal role })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInstrumentPrompt
          Content =
            ChoicePrompt(roleOptions, handleRole character name genre.Id) }
  }

and handleRole character name genre role =
  seq {
    yield
      Prompt
        { Title =
            TextConstant
            <| BandCreatorConfirmationPrompt
                 (character.Name, name, genre, role.Id)
          Content =
            ConfirmationPrompt
            <| handleConfirmation character name genre role }
  }

and handleConfirmation character name genre role confirmed =
  seq {
    if confirmed then
      let band =
        { Name = name
          Genre = genre
          Role = role.Id }

      let result =
        mutate <| StartGameMutation character band

      match result with
      | Ok _ -> yield Scene RehearsalRoom
      | Error CharacterNameTooShort ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorCharacterNameTooShort

              Scene CharacterCreator
            }
      | Error CharacterNameTooLong ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorCharacterNameTooLong

              Scene CharacterCreator
            }
      | Error CharacterAgeTooYoung ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorCharacterAgeTooYoung

              Scene CharacterCreator
            }
      | Error CharacterAgeTooOld ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorCharacterAgeTooOld

              Scene CharacterCreator
            }
      | Error CharacterGenderInvalid ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorCharacterGenderInvalid

              Scene CharacterCreator
            }
      | Error BandNameTooShort ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorBandNameTooShort

              yield! bandCreator character
            }
      | Error BandNameTooLong ->
        yield!
            seq {
              Message
              <| TextConstant CreatorErrorBandNameTooLong

              yield! bandCreator character
            }
      | Error BandGenreInvalid ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorBandGenreInvalid

              yield! handleName character name
            }
      | Error BandRoleInvalid ->
          yield!
            seq {
              Message
              <| TextConstant CreatorErrorBandRoleInvalid

              yield!
                handleGenre character name { Id = genre; Text = Literal genre }
            }
    else
      yield Scene CharacterCreator
  }
