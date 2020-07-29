mod choice_input_display;
mod command_input_display;
mod confirmation_input_display;
mod date_input_display;
mod text_choice_input_display;
mod text_input_display;

use crate::common::action::{CliAction, Prompt};
use crate::common::context::Context;

/// Calls the handler of a specific action and returns the CliAction that
/// was returned from it.
pub fn show(user_action: Prompt, context: &Context) -> CliAction {
    match user_action {
        Prompt::TextInput { text, on_action } => {
            text_input_display::handle(text, on_action, context)
        }
        Prompt::ChoiceInput {
            text,
            choices,
            on_action,
        } => choice_input_display::handle(text, choices, on_action, context),
        Prompt::CommandInput {
            text,
            show_prompt_emoji,
            available_commands,
        } => command_input_display::handle(text, show_prompt_emoji, available_commands, context),
        Prompt::TextChoiceInput {
            text,
            choices,
            on_action,
        } => text_choice_input_display::handle(text, choices, on_action, context),
        Prompt::ConfirmationInput { text, on_action } => {
            confirmation_input_display::handle(text, on_action, context)
        }
        Prompt::DateInput {
            text,
            format,
            on_action,
        } => date_input_display::handle(text, format, on_action, context),
        Prompt::NoOp => CliAction::Prompt(Prompt::NoOp),
    }
}
