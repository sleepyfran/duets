module Cli.View.Scenes.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.TextConstants
open Simulation.Songs.Composition.ComposeSong
open Entities
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
            ChoicePrompt(vocalStyleOptions, handleVocalStyle name length) }
  }

and handleVocalStyle name length selectedVocalStyle =
  let vocalStyle =
    Song.VocalStyle.from selectedVocalStyle.Id

  seq {
    match Song.from name length vocalStyle with
    | Ok song ->
        composeSong song

        yield!
          seq {
            yield
              Message
              <| TextConstant(ComposeSongConfirmation name)

            yield Scene RehearsalRoom
          }
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

and handleError message =
  seq {
    yield Message <| message
    yield! composeSongScene ()
  }
