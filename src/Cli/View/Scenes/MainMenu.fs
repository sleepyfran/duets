module Cli.View.Scenes.MainMenu

open Cli.View.Actions
open Cli.View.TextConstants
open Storage.Savegame

let menuOptions hasSavegameAvailable =
  seq {
    yield
      { Id = "new_game"
        Text = TextConstant MainMenuNewGame }

    if hasSavegameAvailable then
      yield
        { Id = "load_game"
          Text = TextConstant MainMenuLoadGame }

    yield
      { Id = "exit"
        Text = TextConstant MainMenuExit }
  }
  |> List.ofSeq

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu savegameState =
  seq {
    yield Figlet <| TextConstant GameName

    let hasSavegameAvailable =
      match savegameState with
      | Available -> true
      | NotAvailable -> false

    yield
      Prompt
        { Title = TextConstant MainMenuPrompt
          Content =
            ChoicePrompt(menuOptions hasSavegameAvailable, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Scene CharacterCreator)
    | "load_game" -> yield (Scene RehearsalRoom)
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
