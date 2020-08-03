use std::sync::Arc;

use super::Command;
use crate::shared::action::CliAction;
use crate::shared::context::Context;
use crate::shared::display;
use crate::shared::emoji;

/// Allows the user to get time information and interact with the in-game time.
pub fn create_time_command() -> Command {
    Command {
        name: String::from("time"),
        matching_names: vec![],
        explanation: String::from("Shows the time info and allows to advance in time"),
        help: r#"
time
----
When invoked with no further arguments, shows the current time, but can also be invoked
with the following parameters:

advance - Advances one time unit (example: from midday to afternoon)
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            if !args.is_empty() {
                show_time_info(global_context);
                return CliAction::Continue;
            }

            show_time_info(global_context);
            CliAction::Continue
        }),
    }
}

fn show_time_info(global_context: &Context) {
    display::show_text(&get_time_info(global_context))
}

/// Returns the current time info formatted.
pub fn get_time_info(global_context: &Context) -> String {
    let date = global_context.game_state.calendar.date.clone();
    let time = global_context.game_state.calendar.time.clone();

    format!(
        "{} It's currently {}; {}",
        emoji::for_time(&time),
        time.to_string().to_lowercase(),
        date.format("%A,%e %B %Y")
    )
}
