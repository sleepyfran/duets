#[macro_use]
extern crate lazy_static;

mod effects;
mod screens;
mod shared;
mod tests;

use app::operations::start::SavegameState;

use screens::{GameScreen};
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

            display::clear();
            orchestrator::start_with(CliAction::Screen(GameScreen::MainMenu(savegame_state)));
        }
    }
}
