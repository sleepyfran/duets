module Renderer.Text

open View.TextConstants

/// Transforms Game's TextConstants into strings.
let toString constant =
  match constant with
  | MainMenuTitle ->
      "[blue]
.:::::                        .::
.::   .::                     .::
.::    .::.::  .::   .::    .:.: .: .::::
.::    .::.::  .:: .:   .::   .::  .::
.::    .::.::  .::.::::: .::  .::    .:::
.::   .:: .::  .::.:          .::      .::
.:::::      .::.::  .::::      .:: .:: .::
[/]"
  | MainMenuPrompt -> "Select an option to begin"
  | MainMenuNewGame -> "New game"
  | MainMenuLoadGame -> "Load game"
  | MainMenuExit -> "Exit"
