use common::entities::Country;
use common::extensions::option::OptionCloneExtensions;

use crate::context::Context;
use crate::data;

/// Adds functionality to quickly retrieve information about the countries.
pub trait Countries {
    //// Retrieves the information of the current country in which the character is located.
    fn get_current_country(&self) -> Country;
}

impl Countries for Context {
    fn get_current_country(&self) -> Country {
        data::find_in(&self.database.countries, &self.game_state.position.country).unwrap_cloned()
    }
}
