mod choice_input_display;
mod command_input_display;
mod common;
mod date_input_display;
mod text_choice_input_display;
mod text_input_display;

use crate::common::action::{ActionResult, UserAction};

/// Calls the handler of a specific action and returns the ActionResult that
/// was returned from it.
pub fn show(user_action: UserAction) -> ActionResult {
    match &user_action {
        UserAction::TextInput { text, on_action } => text_input_display::handle(&text, &on_action),
        UserAction::ChoiceInput {
            text,
            choices,
            on_action,
        } => choice_input_display::handle(&text, &choices, &on_action),
        UserAction::CommandInput {
            text,
            available_commands,
            on_action,
        } => command_input_display::handle(&text, &available_commands, &on_action),
        UserAction::TextChoiceInput {
            text,
            choices,
            on_action,
        } => text_choice_input_display::handle(&text, &choices, &on_action),
        UserAction::DateInput { text, on_action } => date_input_display::handle(text, on_action),
        UserAction::NoOp => ActionResult::UserAction(UserAction::NoOp),
    }
}
