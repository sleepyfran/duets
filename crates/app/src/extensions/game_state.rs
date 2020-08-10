use common::serializables::GameState;
use engine::entities::{City, Place, Room};

pub trait GameStateExt {
    /// Retrieves all the available cities in the current country.
    fn get_cities(&self) -> Vec<City>;

    /// Retrieves all the available places in the current city.
    fn get_places(&self) -> Vec<Place>;

    /// Retrieves all the available rooms in the current place.
    fn get_rooms(&self) -> Vec<Room>;
}

impl GameStateExt for GameState {
    fn get_cities(&self) -> Vec<City> {
        self.position.country.cities.clone()
    }

    fn get_places(&self) -> Vec<Place> {
        self.position.city.places.clone()
    }

    fn get_rooms(&self) -> Vec<Room> {
        self.position.place.rooms.clone()
    }
}
