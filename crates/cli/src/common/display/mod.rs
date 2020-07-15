mod choice_input_display;
mod common;
mod text_input_display;

use crate::common::action::Action;
use crate::common::action::ActionResult;
use crate::common::screen::Screen;

pub fn show(screen: &Screen) -> ActionResult {
    match &screen.action {
        Action::TextInput { text, on_action } => {
            text_input_display::handle(screen, text, on_action)
        }
        Action::ChoiceInput {
            text,
            choices,
            on_action,
        } => choice_input_display::handle(screen, text, choices, on_action),
        _ => ActionResult::Action(Action::NoOp),
    }
}
