mod create_band;
mod create_character;

use crate::shared::context::Context;
use crate::shared::screen::Screen;

/// Creates a new game screen that handles the creation of the character as well as the first
/// band of the character.
pub fn create_new_game_screen(global_context: &Context) -> Screen {
    Screen {
        name: String::from("New game"),
        action: create_character::start_with_name_input(create_character::create_starting_context(
            global_context,
        )),
    }
}
