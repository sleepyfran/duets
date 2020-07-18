mod choice_input_display;
mod command_input_display;
mod common;
mod date_input_display;
mod text_choice_input_display;
mod text_input_display;

use crate::common::action::{ActionResult, Prompt};

/// Calls the handler of a specific action and returns the ActionResult that
/// was returned from it.
pub fn show(user_action: Prompt) -> ActionResult {
    match &user_action {
        Prompt::TextInput { text, on_action } => text_input_display::handle(&text, &on_action),
        Prompt::ChoiceInput {
            text,
            choices,
            on_action,
        } => choice_input_display::handle(&text, &choices, &on_action),
        Prompt::CommandInput {
            text,
            available_commands,
            on_action,
        } => command_input_display::handle(&text, &available_commands, &on_action),
        Prompt::TextChoiceInput {
            text,
            choices,
            on_action,
        } => text_choice_input_display::handle(&text, &choices, &on_action),
        Prompt::DateInput { text, on_action } => date_input_display::handle(text, on_action),
        Prompt::NoOp => ActionResult::Prompt(Prompt::NoOp),
    }
}
