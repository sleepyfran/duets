module Cli.View.TextConstants

/// Defines all the text constants available in the application. Since this
/// might change between each UI layer (might need custom styling, etc.) the
/// Game layer simply exports these as a type that gets evaluated in each UI.
/// All types must have the screen they belong to (if any) prepended to its name.
type TextConstant =
  | CommonYouAreIn of place: string
  | MainMenuTitle
  | MainMenuPrompt
  | MainMenuNewGame
  | MainMenuLoadGame
  | MainMenuExit
  | MainMenuSavegameNotAvailable
  | CharacterCreatorInitialPrompt
  | CharacterCreatorGenderPrompt
  | CharacterCreatorGenderMale
  | CharacterCreatorGenderFemale
  | CharacterCreatorGenderOther
  | CharacterCreatorAgePrompt
  | BandCreatorInitialPrompt
  | BandCreatorGenrePrompt
  | BandCreatorInstrumentPrompt
  | BandCreatorConfirmationPrompt of
    characterName: string *
    bandName: string *
    bandGenre: string *
    instrument: string
  | RehearsalRoomCompose
  | RehearsalRoomManage
  | RehearsalRoomPrompt
