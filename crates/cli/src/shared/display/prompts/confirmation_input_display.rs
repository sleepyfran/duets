use dialoguer::Confirm;

use crate::shared::action::{CliAction, ConfirmationChoice, PromptText};
use crate::shared::context::Context;

/// Reuses the text_choice_input_display module to avoid repeating the cumbersome process of
/// creating a yes/no input.
pub fn handle(
    text: PromptText,
    on_action: Box<dyn FnOnce(&ConfirmationChoice, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    let text = match text {
        PromptText::WithEmoji(text) => text,
        PromptText::WithoutEmoji(text) => text,
    };

    let confirmed = Confirm::new()
        .with_prompt(text)
        .interact()
        .unwrap_or_default();
    let confirmation_choice = if confirmed {
        ConfirmationChoice::Yes
    } else {
        ConfirmationChoice::No
    };

    on_action(&confirmation_choice, context)
}
