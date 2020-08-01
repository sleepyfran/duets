mod file_system;

use serde_json;

use common::serializables::GameState;

use file_system::FileType;

pub enum Error {
    PathFindingError,
    FileLoadingError,
    FileWritingError,
    FileParsingError,
    ContentParsingError,
}

pub type IoResult<T> = Result<T, Error>;

/// Attempts to load the savegame file and parse its content.
pub fn retrieve_game_state() -> IoResult<GameState> {
    let savegame_content = file_system::read_file(FileType::Savegame)?;
    let game_state =
        serde_json::from_str(&savegame_content).map_err(|_err| Error::FileParsingError)?;

    Ok(game_state)
}

/// Attempts to create or override the savegame file with the given game state.
pub fn save_game_state(game_state: GameState) -> IoResult<()> {
    let serialized_state =
        serde_json::to_string(&game_state).map_err(|_err| Error::ContentParsingError)?;
    file_system::save_file(serialized_state, FileType::Savegame)
}
