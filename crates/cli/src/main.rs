#[macro_use]
extern crate lazy_static;

mod effects;
mod screens;
mod shared;
mod tests;

use app::operations::start::SavegameState;

use screens::main_menu;
use shared::action::CliAction;
use shared::context;
use shared::display;
use shared::orchestrator;

fn main() {
    let savegame_state = app::operations::start::load();

    match &savegame_state {
        SavegameState::InvalidReferences(error) => display::show_error(&error),
        SavegameState::None(context) | SavegameState::Ok(context) => {
            context::set_global_context(context.clone());

            let main_menu_screen = main_menu::create_main_screen(savegame_state);
            display::clear();
            orchestrator::start_with(CliAction::Screen(main_menu_screen));
        }
    }
}
