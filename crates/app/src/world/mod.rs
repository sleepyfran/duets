use common::entities::{City, Country, Object, Place, Room};
use common::extensions::option::OptionCloneExtensions;

use crate::context::Context;
use crate::data;

pub mod interactions;
pub mod movement;

/// Adds functions to quickly retrieve information about the world.
pub trait World {
    /// Retrieves the information of the current country in which the character is located.
    fn get_current_country(&self) -> Country;

    /// Returns all the available cities in the current country in which the character is located.
    fn get_cities_of_current_country(&self) -> Vec<City>;

    /// Returns the current city in which the character is located.
    fn get_current_city(&self) -> City;

    /// Returns all the available places in the current city in which the character is located.
    fn get_places_of_city(&self) -> Vec<Place>;

    /// Returns the current place in which the character is located.
    fn get_current_place(&self) -> Place;

    /// Returns all the available rooms in the current place in which the character is located.
    fn get_rooms_of_place(&self) -> Vec<Room>;

    /// Returns the current room in which the character is located.
    fn get_current_room(&self) -> Room;

    /// Returns all the objects in the current room.
    fn get_objects_in_room(&self) -> Vec<Object>;
}

impl World for Context {
    fn get_current_country(&self) -> Country {
        data::find_in(&self.database.countries, &self.game_state.position.country)
            .cloned()
            .unwrap()
    }

    fn get_cities_of_current_country(&self) -> Vec<City> {
        self.get_current_country().cities
    }

    fn get_current_city(&self) -> City {
        data::find_in(
            &self.get_current_country().cities,
            &self.game_state.position.city,
        )
        .unwrap_cloned()
    }

    fn get_places_of_city(&self) -> Vec<Place> {
        self.get_current_city().places
    }

    fn get_current_place(&self) -> Place {
        data::find_in(
            &self.get_current_city().places,
            &self.game_state.position.place,
        )
        .unwrap_cloned()
    }

    fn get_rooms_of_place(&self) -> Vec<Room> {
        self.get_current_place().rooms
    }

    fn get_current_room(&self) -> Room {
        data::find_in(
            &self.get_current_place().rooms,
            &self.game_state.position.room,
        )
        .unwrap_cloned()
    }

    fn get_objects_in_room(&self) -> Vec<Object> {
        self.get_current_room().objects
    }
}
