module View.Scenes.MainMenu

open Entities.State
open View.Actions
open View.Scenes.Index
open View.TextConstants

let menuOptions =
  [ { Id = "new_game"
      Text = TextConstant MainMenuNewGame }
    { Id = "load_game"
      Text = TextConstant MainMenuLoadGame }
    { Id = "exit"
      Text = TextConstant MainMenuExit } ]

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu state =
  seq {
    yield Message <| TextConstant MainMenuTitle

    yield
      Prompt
        { Title = TextConstant MainMenuPrompt
          Content = ChoicePrompt(menuOptions, processSelection state) }
  }

and processSelection (state: State) choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Scene CharacterCreator)
    | "load_game" ->
        if state.Initialized then
          yield! []
        else
          yield
            Message
            <| TextConstant MainMenuSavegameNotAvailable

          yield Scene MainMenu
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
