module View.Scenes.MainMenu

open View.Actions
open View.TextConstants

let menuOptions =
  [ { Id = "new_game"
      Text = MainMenuNewGame }
    { Id = "load_game"
      Text = MainMenuLoadGame }
    { Id = "exit"; Text = MainMenuExit } ]

let idsFrom options =
  List.map (fun choice -> choice.Id) options

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu () =
  seq {
    yield Message MainMenuTitle

    yield
      Prompt
        { Title = MainMenuPrompt
          Content = ChoicePrompt(menuOptions, processSelection) }
  }

and processSelection choice =
  seq {
    match choice.Id with
    | "new_game" -> yield (Message MainMenuNewGame)
    | "load_game" -> yield! []
    | "exit" -> yield NoOp
    | _ -> yield NoOp
  }
