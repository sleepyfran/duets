module Cli.View.TextConstants

open Entities

/// Defines all the text constants available in the application. Since this
/// might change between each UI layer (might need custom styling, etc.) the
/// Game layer simply exports these as a type that gets evaluated in each UI.
/// All types must have the screen they belong to (if any) prepended to its name.
type TextConstant =
  | GameName
  | CommonYouAreIn of place: string
  | CommonNoUnfinishedSongs
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
  | CreatorErrorCharacterNameTooShort
  | CreatorErrorCharacterNameTooLong
  | CreatorErrorCharacterAgeTooYoung
  | CreatorErrorCharacterAgeTooOld
  | CreatorErrorBandNameTooShort
  | CreatorErrorBandNameTooLong
  | RehearsalRoomTitle
  | RehearsalRoomCompose
  | RehearsalRoomManage
  | RehearsalRoomPrompt
  | ComposePrompt
  | ComposeSong
  | ComposeSongTitlePrompt
  | ComposeSongLengthPrompt
  | ComposeSongVocalStylePrompt
  | ComposeSongConfirmation of title: string
  | ComposeSongErrorNameTooShort
  | ComposeSongErrorNameTooLong
  | ComposeSongErrorLengthTooShort
  | ComposeSongErrorLengthTooLong
  | ComposeSongProgressBrainstorming
  | ComposeSongProgressTryingChords
  | ComposeSongProgressConfiguringReverb
  | ImproveSong
  | ImproveSongSelection
  | ImproveSongCanBeFurtherImproved of quality: Quality
  | ImproveSongReachedMaxQuality of quality: Quality
  | ImproveSongProgressAddingSomeMelodies
  | ImproveSongProgressPlayingFoosball
  | ImproveSongProgressModifyingChordsFromAnotherSong
  | FinishSong
  | FinishSongSelection
  | FinishSongFinished of name: string * quality: Quality
  | DiscardSong
  | DiscardSongSelection
  | DiscardSongDiscarded of name: string
  | PracticeSong
