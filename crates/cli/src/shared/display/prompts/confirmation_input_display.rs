use dialoguer::Confirm;

use crate::shared::action::{CliAction, ConfirmationChoice};
use crate::shared::context::Context;

/// Reuses the text_choice_input_display module to avoid repeating the cumbersome process of
/// creating a yes/no input.
pub fn handle(
    text: String,
    on_action: Box<dyn FnOnce(&ConfirmationChoice, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
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
