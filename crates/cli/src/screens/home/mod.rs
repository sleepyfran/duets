use super::new_game;
use crate::common::action::Choice;
use crate::common::action::CliAction;
use crate::common::action::Prompt;
use crate::common::context::Context;
use crate::common::screen::Screen;
use crate::effects;

/// Home screen is the main place where the user can interact with the rest of the game by giving
/// different commands.
pub fn create_home_screen(global_context: &Context) -> Screen {
    Screen {
        name: String::from("Home"),
        action: Prompt::CommandInput {
            text: home_current_info_text(global_context),
            avaiable_commands: vec![],
            on_action: Box::new(|choice, global_context| match choice.id {
                0 => CliAction::Screen(new_game::create_new_game_screen(global_context)),
                1 => CliAction::SideEffect(effects::exit),
                2 => CliAction::SideEffect(effects::exit),
                _ => CliAction::Prompt(Prompt::NoOp),
            }),
        },
    }
}

fn home_current_info_text(global_context) -> String {

}
