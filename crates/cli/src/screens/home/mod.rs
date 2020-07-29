use crate::common::action::Prompt;
use crate::common::context::Context;
use crate::common::emoji;
use crate::common::screen::Screen;

/// Home screen is the main place where the user can interact with the rest of the game by giving
/// different commands.
pub fn create_home_screen(global_context: &Context) -> Screen {
    Screen {
        name: String::from("Home"),
        action: Prompt::CommandInput {
            text: home_current_info_text(global_context),
            show_prompt_emoji: false,
            available_commands: vec![],
        },
    }
}

fn home_current_info_text(global_context: &Context) -> String {
    let time = global_context.game_state.calendar.time.clone();
    let time_info = format!(
        "{} It's currently {}",
        emoji::clock_emoji_for_time(&time),
        time.to_string().to_lowercase()
    );

    let position_info = format!(
        "📍 You're currently in {}",
        global_context.game_state.current_city.name,
    );

    let command_info = format!(
        "💬 Write some command to execute an action! You can also write help to show the list of available commands"
    );

    format!("{}\n{}\n{}", time_info, position_info, command_info)
}
