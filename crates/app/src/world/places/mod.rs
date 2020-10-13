use common::entities::Place;
use common::extensions::option::OptionCloneExtensions;

use crate::context::Context;
use crate::data;
use crate::world::cities::Cities;

/// Adds functionality to quickly retrieve information about the places.
pub trait Places {
    /// Returns the current place in which the character is located.
    fn get_current_place(&self) -> Place;

    /// Returns all the available places in the current city in which the character is located.
    fn get_places_of_city(&self) -> Vec<Place>;
}

impl Places for Context {
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
}
