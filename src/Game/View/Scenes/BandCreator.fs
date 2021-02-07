module View.Scenes.BandCreator

open Data.Queries
open Entities.Band
open Entities.Calendar
open View.Actions
open View.TextConstants
open View.Scenes.Index

let rec bandCreator character =
  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInitialPrompt
          Content = TextPrompt <| handleName character }
  }

and handleName character name =
  let band = { getDefault () with Name = name }

  let genreOptions =
    Genres.getAll ()
    |> List.map (fun genre -> { Id = genre; Text = String genre })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorGenrePrompt
          Content = ChoicePrompt(genreOptions, handleGenre character band) }
  }

and handleGenre character band genre =
  let band = { band with Genre = genre.Id }

  let roleOptions =
    Roles.getNames ()
    |> List.map (fun role -> { Id = role; Text = String role })

  seq {
    yield
      Prompt
        { Title = TextConstant BandCreatorInstrumentPrompt
          Content = ChoicePrompt(roleOptions, handleRole character band) }
  }

and handleRole character band role =
  let band =
    { band with
        Members =
          [ (character, toRole role.Id, (fromDayMonth 1 1, Ongoing)) ] }

  seq {
    yield
      Prompt
        { Title =
            TextConstant
            <| BandCreatorConfirmationPrompt(
              character.Name,
              band.Name,
              band.Genre,
              role.Id
            )
          Content = ConfirmationPrompt <| handleConfirmation band }
  }

and handleConfirmation band confirmed =
  seq {
    if confirmed then
      yield Effect <| fun state -> { state with Band = band }

      yield Message <| String "Welcome!"
    else
      yield Scene CharacterCreator
  }
