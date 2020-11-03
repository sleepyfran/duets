mod home;
mod interaction;
mod main_menu;
mod new_game;

use std::rc::Rc;

use game::context::Context;
use game::operations::start::SavegameState;
use game::world::interactions::{InteractItem, Interaction};

use crate::shared::screen::Screen;

/// Lists all the available screens in the game. Those screens that require any extra parameters
/// to be passed will expose them as extra data in the variant.
pub enum GameScreen {
    MainMenu(SavegameState),
    NewGame,
    Home,
    Interaction {
        interaction: Rc<dyn Interaction>,
        sequence: InteractItem,
        context: Context,
    },
}

/// Given a screen to instantiate and the current context, creates the given screen with the proper
/// context and any other necessary parameters.
pub fn create(screen: GameScreen, context: &Context) -> Screen {
    match screen {
        GameScreen::MainMenu(savegame_state) => main_menu::create_main_screen(savegame_state),
        GameScreen::Home => home::create_home_screen(context),
        GameScreen::NewGame => new_game::create_new_game_screen(context),
        GameScreen::Interaction {
            interaction,
            sequence,
            context,
        } => interaction::create_interaction_screen(interaction, sequence, context),
    }
}
