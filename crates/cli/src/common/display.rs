use crate::common::action::Action;
use crate::common::action::ActionResult;
use crate::common::screen::Screen;

pub fn show(screen: Screen) -> ActionResult {
    match screen.action {
        Action::TextInput { text, on_action } => {
            let input = showTextInputAction(text);
            return on_action(&input, &screen);
        }
    }
}

fn showTextInputAction(text: String) -> String {
    return String::from("");
}
