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
  | CharacterCreatorInitialPrompt ->
      "Creating a new game, what's the name of your character?"
  | CharacterCreatorGenderPrompt -> "What's their gender?"
  | CharacterCreatorGenderMale -> "Male"
  | CharacterCreatorGenderFemale -> "Female"
  | CharacterCreatorGenderOther -> "Other"
  | CharacterCreatorAgePrompt -> "How old are they?"
