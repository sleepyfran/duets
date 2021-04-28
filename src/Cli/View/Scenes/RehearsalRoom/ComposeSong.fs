module Cli.View.Scenes.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants
open Entities
open FSharp.Data.UnitSystems.SI.UnitNames
open Simulation.Songs.Composition.ComposeSong
open Storage.Database

let rec composeSongScene () =
  seq {
    yield
      Prompt
        { Title = TextConstant ComposeSongTitlePrompt
          Content = TextPrompt handleName }
  }

and handleName name =
  seq {
    yield
      Prompt
        { Title = TextConstant ComposeSongLengthPrompt
          Content = NumberPrompt(handleLength name) }
  }

and handleLength name length =
  seq {
    yield
      Prompt
        { Title = TextConstant ComposeSongGenrePrompt
          Content = ChoicePrompt(genreOptions, handleGenre name length) }
  }

and handleGenre name length selectedGenre =
  let vocalStyleOptions =
    vocalStyleNames ()
    |> List.map
         (fun vocalStyle ->
           { Id = vocalStyle.ToString()
             Text = Literal(vocalStyle.ToString()) })

  seq {
    yield
      Prompt
        { Title = TextConstant ComposeSongVocalStylePrompt
          Content =
            ChoicePrompt(
              vocalStyleOptions,
              handleVocalStyle name (length * 1<second>) selectedGenre.Id
            ) }
  }

and handleVocalStyle name length genre selectedVocalStyle =
  let vocalStyle =
    Song.VocalStyle.from selectedVocalStyle.Id

  seq {
    match Song.from name length vocalStyle genre with
    | Ok song -> yield! composeWithProgressbar song
    | Error Song.NameTooShort ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorNameTooShort
    | Error Song.NameTooLong ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorNameTooLong
    | Error Song.LengthTooShort ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorLengthTooShort
    | Error Song.LengthTooLong ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorLengthTooLong
  }

and composeWithProgressbar song =
  seq {
    composeSong song

    yield
      ProgressBar
        { StepNames =
            [ TextConstant ComposeSongProgressBrainstorming
              TextConstant ComposeSongProgressConfiguringReverb
              TextConstant ComposeSongProgressTryingChords ]
          StepDuration = 2<second>
          Async = true }

    yield
      Message
      <| TextConstant(ComposeSongConfirmation song.Name)

    yield Scene RehearsalRoom
  }

and handleError message =
  seq {
    yield Message <| message
    yield! composeSongScene ()
  }
