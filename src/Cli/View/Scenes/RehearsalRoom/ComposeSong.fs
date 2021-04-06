module Cli.View.Scenes.RehearsalRoom.ComposeSong

open Cli.View.Actions
open Cli.View.TextConstants
open Mediator.Mutation
open Mediator.Mutations.Songs
open Mediator.Query
open Mediator.Queries.Storage

let rec composeSong () =
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
    query VocalStylesQuery
    |> List.map (fun vocalStyle ->
         { Id = vocalStyle
           Text = Literal vocalStyle })

  seq {
    yield
      Prompt
        { Title = TextConstant ComposeSongVocalStylePrompt
          Content =
            ChoicePrompt(vocalStyleOptions, handleVocalStyle name length) }
  }

and handleVocalStyle name length vocalStyle =
  let result =
    mutate
    <| ComposeSongMutation
         { Name = name
           Length = length
           VocalStyle = vocalStyle.Id }

  seq {
    match result with
    | Ok _ ->
        yield!
          seq {
            yield
              Message
              <| TextConstant(ComposeSongConfirmation name)

            yield Scene RehearsalRoom
          }
    | Error NameTooShort ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorNameTooShort
    | Error NameTooLong ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorNameTooLong
    | Error LengthTooShort ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorLengthTooShort
    | Error LengthTooLong ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorLengthTooLong
    | Error VocalStyleInvalid ->
        yield!
          handleError
          <| TextConstant ComposeSongErrorVocalStyleInvalid
  }

and handleError message =
  seq {
    yield Message <| message
    yield! composeSong ()
  }
