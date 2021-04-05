module Text

open System
open Cli.View.Actions
open Cli.View.TextConstants

/// Transforms Game's TextConstants into strings.
let rec toString text =
  match text with
  | TextConstant constant -> fromConstant constant
  | String string -> string

and fromConstant constant =
  match constant with
  | CommonYouAreIn (place) -> String.Format("You're currently in {0}", place)
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
  | MainMenuSavegameNotAvailable ->
      "[red]No savegame available. Create a new game[/]"
  | CharacterCreatorInitialPrompt ->
      "Creating a new game, what's the name of your character?"
  | CharacterCreatorGenderPrompt -> "What's their gender?"
  | CharacterCreatorGenderMale -> "Male"
  | CharacterCreatorGenderFemale -> "Female"
  | CharacterCreatorGenderOther -> "Other"
  | CharacterCreatorAgePrompt -> "How old are they?"
  | BandCreatorInitialPrompt ->
      "Let's create your first band. What's the band's name?"
  | BandCreatorGenrePrompt ->
      "What genre are they going to be playing? You can always change this later"
  | BandCreatorInstrumentPrompt -> "And lastly, what will you be playing?"
  | BandCreatorConfirmationPrompt (characterName,
                                   bandName,
                                   bandGenre,
                                   instrument) ->
      String.Format
        ("You'll be playing as {0} in the band {1} playing {2}",
         characterName,
         bandName,
         bandGenre,
         instrument)
  | RehearsalRoomCompose -> "Compose"
  | RehearsalRoomManage -> "Manage band"
  | RehearsalRoomPrompt -> "What do you want to do today?"
