module View.Scenes.BandCreator

open Mediator.Mutations.Setup
open View.Actions
open View.TextConstants

let rec bandCreator character =
  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInitialPrompt
          Content = TextPrompt <| handleName character }
  }

and handleName character name =
  let genreOptions =
    Genres.getAll ()
    |> List.map (fun genre -> { Id = genre; Text = String genre })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorGenrePrompt
          Content = ChoicePrompt(genreOptions, handleGenre character name) }
  }

and handleGenre character name genre =
  let roleOptions =
    Roles.getNames ()
    |> List.map (fun role -> { Id = role; Text = String role })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInstrumentPrompt
          Content = ChoicePrompt(roleOptions, handleRole character name genre.Id) }
  }

and handleRole character name genre role =
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
          Content = ConfirmationPrompt <| handleConfirmation character name genre role }
  }

and handleConfirmation character name genre role confirmed =
  seq {
    if confirmed then
      // TODO: Call command on Mediator.
      
      yield Message <| String "Welcome!"
    else
      yield Scene CharacterCreator
  }
