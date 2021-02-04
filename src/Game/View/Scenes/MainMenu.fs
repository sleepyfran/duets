module View.Scenes.MainMenu

open View.Actions

let menuOptions =
  [ { Id = "new_game"; Text = "New game" }
    { Id = "load_game"; Text = "Load game" }
    { Id = "exit"; Text = "Exit" } ]

let idsFrom options =
  List.map (fun choice -> choice.Id) options

/// Creates the main menu of the game as a sequence of actions.
let rec mainMenu () =
  seq {
    yield
      Message
        "
         .:::::                        .::
         .::   .::                     .::
         .::    .::.::  .::   .::    .:.: .: .::::
         .::    .::.::  .:: .:   .::   .::  .::
         .::    .::.::  .::.::::: .::  .::    .:::
         .::   .:: .::  .::.:          .::      .::
         .:::::      .::.::  .::::      .:: .:: .::
         "

    yield
      Prompt
        { Title = "Select an option to begin"
          Content = ChoicePrompt(menuOptions, processSelection) }
  }

and processSelection choice =
  let validSelection =
    List.contains choice.Id (idsFrom menuOptions)

  seq {
    if validSelection then
      match choice.Id with
      | "new_game" -> yield! []
      | "load_game" -> yield! []
      | "exit" -> yield! []
      | _ -> yield NoOp
    else
      yield! mainMenu ()
  }
