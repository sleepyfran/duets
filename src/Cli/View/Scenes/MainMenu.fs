module Cli.View.Scenes.MainMenu

open Mediator.Query
open Mediator.Queries.Storage
open Cli.View.Actions
open Cli.View.TextConstants

let menuOptions =
  [ { Id = "new_game"
      Text = TextConstant MainMenuNewGame }
    { Id = "load_game"
      Text = TextConstant MainMenuLoadGame }
    { Id = "exit"
      Text = TextConstant MainMenuExit } ]

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu () =
  seq {
    yield Message <| TextConstant MainMenuTitle

    yield
      Prompt
        { Title = TextConstant MainMenuPrompt
          Content = ChoicePrompt(menuOptions, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Scene CharacterCreator)
    | "load_game" ->
        let savegameAvailable = query SavegameStateQuery

        match savegameAvailable with
        | Available -> yield! []
        | NotAvailable ->
            yield
              Message
              <| TextConstant MainMenuSavegameNotAvailable

            yield Scene MainMenu
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
