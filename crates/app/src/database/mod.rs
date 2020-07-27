mod loader;

pub use loader::*;

use serde::{Deserialize, Deserializer};
use serde_json;

use engine::entities::Country;

use crate::serializables::CountryDef;

/// The game database represents the read only data that is remotely fetched and that holds the
/// static data of the game such as countries, cities, instruments, etc. This should be initialized
/// when starting the game and cached so the user doesn't have to constantly download the database
/// every time they open the game.
#[derive(Clone, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct Database {
    pub compatible_with: String,
    #[serde(deserialize_with = "vec_country")]
    pub countries: Vec<Country>,
}

/// Generic error when loading the database.
#[derive(Debug, Clone)]
pub struct DatabaseLoadError {
    description: String,
}

/// We need to define a custom deserializer because Serde does not support containers right now.
///
/// Tracking issue: https://github.com/serde-rs/serde/issues/723
/// Taken from: https://github.com/serde-rs/serde/issues/723#issuecomment-382501277
fn vec_country<'de, D>(deserializer: D) -> Result<Vec<Country>, D::Error>
where
    D: Deserializer<'de>,
{
    #[derive(Deserialize)]
    struct Wrapper(#[serde(with = "CountryDef")] Country);

    let v = Vec::deserialize(deserializer)?;
    Ok(v.into_iter().map(|Wrapper(a)| a).collect())
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
