use common::serializables::GameState;
use engine::entities::{City, Country, Object, Place, Room};

use crate::constants;
use crate::database::Database;

/// Defines the current context of the game and adds some utility methods to query information
/// about the current status.
#[derive(Clone, Default)]
pub struct Context {
    pub database: Database,
    pub game_state: GameState,
}

impl Context {
    /// Validates that all the references exist in the database. This is incredibly important
    /// since it can lead to undefined behavior later on if the savegame is corrupted.
    ///
    /// Should only be called after initializing the game_state to a normal state, since otherwise
    /// it'll fail.
    pub fn validate_integrity(&self) -> Result<(), String> {
        self.get_current_country_opt()
            .and_then(|_| self.get_current_city_opt())
            .and_then(|_| self.get_current_place_opt())
            .and_then(|_| self.get_current_room_opt())
            .map(|_| {})
            .ok_or(constants::errors::savegame::INVALID_ID_REFERENCE.into())
    }

    fn get_current_country_opt(&self) -> Option<Country> {
        self.database
            .countries
            .iter()
            .cloned()
            .find(|country| country.id == self.game_state.position.country.id)
    }

    fn get_current_city_opt(&self) -> Option<City> {
        self.get_cities_of_current_country()
            .into_iter()
            .find(|city| city.id == self.game_state.position.city.id)
    }

    fn get_current_place_opt(&self) -> Option<Place> {
        self.get_places_of_city()
            .into_iter()
            .find(|place| place.id == self.game_state.position.place.id)
    }

    fn get_current_room_opt(&self) -> Option<Room> {
        self.get_rooms_of_place()
            .into_iter()
            .find(|room| room.id == self.game_state.position.room.id)
    }

    /// Retrieves the information of the current country in which the character is located.
    pub fn get_current_country(&self) -> Country {
        self.get_current_country_opt().unwrap()
    }

    /// Returns all the available cities in the current country in which the character is located.
    pub fn get_cities_of_current_country(&self) -> Vec<City> {
        self.get_current_country().cities
    }

    /// Returns the current city in which the character is located.
    pub fn get_current_city(&self) -> City {
        self.get_current_city_opt().unwrap()
    }

    /// Returns all the available places in the current city in which the character is located.
    pub fn get_places_of_city(&self) -> Vec<Place> {
        self.get_current_city().places
    }

    /// Returns the current place in which the character is located.
    pub fn get_current_place(&self) -> Place {
        self.get_current_place_opt().unwrap()
    }

    /// Returns all the available rooms in the current place in which the character is located.
    pub fn get_rooms_of_place(&self) -> Vec<Room> {
        self.get_current_place().rooms
    }

    /// Returns the current room in which the character is located.
    pub fn get_current_room(&self) -> Room {
        self.get_current_room_opt().unwrap()
    }

    /// Returns all the objects in the current room.
    pub fn get_objects_in_room(&self) -> Vec<Object> {
        self.get_current_room().objects
    }
}
