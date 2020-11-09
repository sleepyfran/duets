use serde::Deserialize;

use common::entities::{Country, Genre, Instrument};

/// Defines what went wrong while trying to load the database.
#[derive(Debug, Clone)]
pub enum DatabaseLoaderError {
    NoConnection,
    DatabaseCorrupted,
}

/// The game database represents the read only data that is remotely fetched and that holds the
/// static data of the game such as countries, cities, instruments, etc. This should be initialized
/// when starting the game and cached so the user doesn't have to constantly download the database
/// every time they open the game.
#[derive(Clone, Deserialize, Default)]
#[serde(rename_all = "camelCase")]
pub struct Database {
    pub countries: Vec<Country>,
    pub genres: Vec<Genre>,
    pub instruments: Vec<Instrument>,
}

/// Generic error when loading the database.
#[derive(Debug, Clone)]
pub struct DatabaseLoadError {
    pub description: String,
}

impl Database {
    /// Parses the given JSON and transforms it into our internal representation of the database.
    /// Returns an error if some field is missing or the database is not compatible with the current
    /// version of the game.
    pub fn init_with(json: String) -> Result<Database, DatabaseLoadError> {
        let database = try_deserialize(json)?;
        Ok(database)
    }
}

fn try_deserialize(json: String) -> Result<Database, DatabaseLoadError> {
    let database_or_error = serde_json::from_str(&json);
    match database_or_error {
        Ok(database) => Ok(database),
        Err(err) => Err(DatabaseLoadError {
            description: err.to_string(),
        }),
    }
}
