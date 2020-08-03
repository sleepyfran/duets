use crate::shared::action::{CliAction, Prompt};
use crate::shared::commands::{character, time, CommandCollection};
use crate::shared::context::Context;
use crate::shared::screen::Screen;

/// Home screen is the main place where the user can interact with the rest of the game by giving
/// different commands.
pub fn create_home_screen(previous_global_context: &Context) -> Screen {
    Screen {
        name: String::from("Home"),
        action: Prompt::CommandInput {
            text: home_current_info_text(previous_global_context),
            show_prompt_emoji: false,
            available_commands: CommandCollection::default()
                .add(character::create_character_command())
                .add(time::create_time_command())
                .clone(),
            after_action: Box::new(|_command, global_context| {
                CliAction::Screen(create_home_screen(global_context))
            }),
        },
    }
}

fn home_current_info_text(global_context: &Context) -> String {
    let time_info = time::get_time_info(global_context);

    let position_info = format!(
        "📍 You're currently in {}",
        global_context.game_state.current_city.name,
    );

    let command_info = format!(
        "💬 Write some command to execute an action! You can also write help to show the list of available commands"
    );

    format!("{}\n{}\n{}", time_info, position_info, command_info)
}
