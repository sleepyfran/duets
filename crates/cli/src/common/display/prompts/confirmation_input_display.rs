use super::text_choice_input_display;

use crate::common::action::{Choice, CliAction, ConfirmationChoice};
use crate::common::context::Context;

/// Reuses the text_choice_input_display module to avoid repeating the cumbersome process of
/// creating a yes/no input.
pub fn handle(
    text: String,
    on_action: Box<dyn FnOnce(&ConfirmationChoice, &Context) -> CliAction>,
    context: &Context,
) -> CliAction {
    text_choice_input_display::handle(
        text,
        vec![
            Choice {
                id: 0,
                text: String::from("Yes"),
            },
            Choice {
                id: 1,
                text: String::from("No"),
            },
        ],
        Box::new(|choice, global_context| {
            let confirmation_choice = if choice.id == 0 {
                ConfirmationChoice::Yes
            } else {
                ConfirmationChoice::No
            };

            on_action(&confirmation_choice, global_context)
        }),
        context,
    )
}
