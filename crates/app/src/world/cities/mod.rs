use common::entities::City;
use common::extensions::option::OptionCloneExtensions;

use crate::context::Context;
use crate::data;
use crate::world::countries::Countries;

/// Adds functionality to quickly retrieve information about the cities.
pub trait Cities {
    /// Returns all the available cities in the current country in which the character is located.
    fn get_cities_of_current_country(&self) -> Vec<City>;

    /// Returns the current city in which the character is located.
    fn get_current_city(&self) -> City;
}

impl Cities for Context {
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
}
