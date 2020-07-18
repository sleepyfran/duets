use crate::common::action::ActionResult;
use crate::common::display;
use crate::common::display::prompts;

/// Recursively calls itself with the result of the given ActionResult. This
/// will create an infinite loop until we decide to exit using the exit side
/// effect, effectively showing each screen and actions as they're connected.
pub fn start(action: ActionResult) {
    let result = match action {
        ActionResult::Screen(screen) => Some(display::show(screen)),
        ActionResult::SideEffect(effect) => effect(),
        ActionResult::Prompt(user_action) => Some(prompts::show(user_action)),
    };

    display::show_line_break();

    match result {
        Some(action_result) => start(action_result),
        None => display::show_exit_message(),
    }
}
