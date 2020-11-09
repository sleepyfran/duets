use common::entities::GameState;

use super::database::{Database, DatabaseLoadError};
use super::integrity;
use super::savegame;
use crate::context::Context;

/// Different states in which a savegame can be once it is loaded from disc. If None it means that
/// there is no savegame that can be loaded. When InvalidReferences it indicates that the savegame
/// contains references to some entity that does not exist in the database.
pub enum SavegameState {
    None(Context),
    InvalidReferences(String),
    Ok(Context),
}

/// Retrieves everything needed for the start of the game.
pub fn load() -> SavegameState {
    let database = load_database().unwrap();
    let game_state = savegame::load().ok();

    match game_state {
        Some(game_state) => from_savegame(database, game_state),
        None => from_scratch(database),
    }
}

fn load_database() -> Result<Database, DatabaseLoadError> {
    storage::retrieve_database()
        .map_err(|_| DatabaseLoadError {
            description: "Error loading bundled database".into(),
        })
        .and_then(Database::init_with)
}

fn from_savegame(database: Database, game_state: GameState) -> SavegameState {
    match integrity::validate(&database, &game_state) {
        Err(error) => SavegameState::InvalidReferences(error),
        _ => SavegameState::Ok(Context {
            database,
            game_state,
        }),
    }
}

fn from_scratch(database: Database) -> SavegameState {
    let context = Context {
        database,
        game_state: GameState::default(),
    };

    SavegameState::None(context)
}
