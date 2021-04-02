module View.Scenes.MainMenu

open View.Actions
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

and processSelection (state: State option) choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Scene CharacterCreator)
    | "load_game" ->
        match state with
        | Some _ -> yield! []
        | None -> 
          yield Message <| TextConstant MainMenuSavegameNotAvailable
          yield Scene MainMenu
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
