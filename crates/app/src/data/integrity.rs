use common::entities::GameState;

use crate::constants;
use crate::data::database::Database;
use crate::data::find_in;

/// Validates that all the references exist in the database. This is incredibly important
/// since it can lead to undefined behavior later on if the savegame is corrupted.
pub fn validate(database: &Database, game_state: &GameState) -> Result<(), String> {
    find_in(&database.countries, &game_state.position.country)
        .and_then(|country| find_in(&country.cities, &game_state.position.city))
        .and_then(|city| find_in(&city.places, &game_state.position.place))
        .and_then(|place| find_in(&place.rooms, &game_state.position.room))
        .map(|_| {})
        .ok_or_else(|| constants::errors::savegame::INVALID_ID_REFERENCE.into())
}
