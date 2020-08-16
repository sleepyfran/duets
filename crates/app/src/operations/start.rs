use common::serializables::GameState;
use storage;

use crate::context::Context;
use crate::database::{Database, DatabaseLoadError};

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
    let game_state = storage::retrieve_game_state().ok();

    match game_state {
        Some(game_state) => from_savegame(database, game_state),
        None => from_scratch(database),
    }
}

fn load_database() -> Result<Database, DatabaseLoadError> {
    // TODO: Load the database from the server and not from a mock.
    Database::init_with(
        r#"
            {
                "compatibleWith": "0.1.0",
                "countries": [
                    {
                        "id": "CZK",
                        "name": "Czech Republic",
                        "population": 10690000,
                        "cities": [
                            {
                                "id": "PRG",
                                "name": "Prague",
                                "population": 1309000,
                                "places": [
                                    {
                                        "id": "test_1",
                                        "name": "Test Place",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test",
                                                "objects": [
                                                    {
                                                        "id": "test_obj_1",
                                                        "name": "guitar",
                                                        "description": "A simple guitar for testing",
                                                        "type": {
                                                            "Instrument": {
                                                                "name": "Guitar",
                                                                "allowsAnotherInstrument": true
                                                            }
                                                        }
                                                    },
                                                    {
                                                        "id": "test_obj_2",
                                                        "name": "fake guitar",
                                                        "description": "A fake guitar to test spaces in names",
                                                        "type": {
                                                            "Instrument": {
                                                                "name": "Guitar",
                                                                "allowsAnotherInstrument": true
                                                            }
                                                        }
                                                    }
                                                ]
                                            }
                                        ]
                                    },
                                    {
                                        "id": "test_2",
                                        "name": "Test Place 2",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test, but again",
                                                "objects": []
                                            }
                                        ]
                                    },
                                    {
                                        "id": "test_3",
                                        "name": "Test Place 3",
                                        "rooms": [
                                            {
                                                "id": "test_1",
                                                "description": "A dark room just to test, but again",
                                                "objects": []
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ],
                "genres": [
                    {
                        "name": "Blackgaze",
                        "compatibleWith": []
                    }
                ],
                "instruments": [
                    {
                        "name": "Guitar",
                        "allowsAnotherInstrument": true
                    }
                ]
            }
        "#
        .to_string(),
    )
}

fn from_savegame(database: Database, game_state: GameState) -> SavegameState {
    let context = Context {
        database: database,
        game_state: game_state.clone(),
    };

    match context.validate_integrity() {
        Err(error) => SavegameState::InvalidReferences(error),
        _ => SavegameState::Ok(context),
    }
}

fn from_scratch(database: Database) -> SavegameState {
    let context = Context {
        database: database,
        game_state: GameState::default(),
    };

    SavegameState::None(context)
}
