module Text

open System
open Cli.View.Actions
open Cli.View.TextConstants

/// Transforms TextConstants into strings.
/// TODO: Move all this to a JSON file or other easier to edit format.
let rec toString text =
  match text with
  | TextConstant constant -> fromConstant constant
  | Literal string -> string

and fromConstant constant =
  match constant with
  | CommonYouAreIn (place) -> String.Format("You're currently in {0}", place)
  | CommonNoUnfinishedSongs -> "You don't have any songs, create one first"
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
  | CharacterCreatorAgePrompt -> "How old are they? (Minimum 18)"
  | BandCreatorInitialPrompt ->
      "Let's create your first band. What's the band's name?"
  | BandCreatorGenrePrompt ->
      "What genre are they going to be playing? You can always change this later"
  | BandCreatorInstrumentPrompt -> "And lastly, what will you be playing?"
  | BandCreatorConfirmationPrompt (characterName,
                                   bandName,
                                   bandGenre,
                                   instrument) ->
      String.Format(
        "You'll be playing as {0} in the band {1} playing {2}",
        characterName,
        bandName,
        bandGenre,
        instrument
      )
  | CreatorErrorCharacterNameTooShort ->
      "[red]Your character's name is too short[/]"
  | CreatorErrorCharacterNameTooLong ->
      "[red]Your character's name is too long[/]"
  | CreatorErrorCharacterAgeTooYoung -> "[red]Your character is too young[/]"
  | CreatorErrorCharacterAgeTooOld -> "[red]Your character is too old[/]"
  | CreatorErrorCharacterGenderInvalid ->
      "[red]Your character's gender is invalid[/]"
  | CreatorErrorBandNameTooShort -> "[red]Your band's name is too short[/]"
  | CreatorErrorBandNameTooLong -> "[red]Your band's name is too long[/]"
  | CreatorErrorBandGenreInvalid -> "[red]Your band's genre is invalid[/]"
  | CreatorErrorBandRoleInvalid -> "[red]Your band's role is invalid[/]"
  | RehearsalRoomCompose -> "Compose"
  | RehearsalRoomManage -> "Manage band"
  | RehearsalRoomPrompt -> "What do you want to do today?"
  | ComposePrompt -> "What do you want to compose?"
  | ComposeSong -> "Compose new song"
  | ComposeSongTitlePrompt ->
      "Creating a new song, how are you going to name it?"
  | ComposeSongLengthPrompt -> "How long is it going to be? (in seconds)"
  | ComposeSongVocalStylePrompt -> "What vocal style should it have?"
  | ComposeSongConfirmation (title) ->
      String.Format(
        "Your band has started working on the song {0}. You can finish or improve it through the compose section in the rehearsal room",
        title
      )
  | ComposeSongErrorNameTooShort -> "[red]The name of the song is too short[/]"
  | ComposeSongErrorNameTooLong -> "[red]The name of the song is too long[/]"
  | ComposeSongErrorLengthTooShort ->
      "[red]Songs can't be less than 20 seconds long[/]"
  | ComposeSongErrorLengthTooLong ->
      "[red]Songs can't be more than 30 minutes long[/]"
  | ComposeSongErrorVocalStyleInvalid -> "[red]The vocal style is invalid[/]"
  | ImproveSong -> "Improve an unfinished song"
  | ImproveSongSelection -> "Which song do you want to improve?"
  | ImproveSongCanBeFurtherImproved quality ->
      String.Format(
        "You've improved the song. It now has a quality of {0}%",
        quality
      )
  | ImproveSongReachedMaxQuality quality ->
      String.Format(
        "Your band has decided that the song does not need any further improvements. It has a quality of {0}%. You can add it to the band's repertoire from the 'Finish an unfinished song' option",
        quality
      )
  | FinishSong -> "Finish an unfinished song"
  | FinishSongSelection ->
      "Which song do you want to finish? [red]You won't be able to improve the song after this[/]"
  | FinishSongFinished (name, quality) ->
      String.Format(
        "Your band finished the song {0}. The result quality is {1}",
        name,
        quality
      )
  | DiscardSong -> "Discard an unfinished song"
  | PracticeSong -> "Practice a finished song"
