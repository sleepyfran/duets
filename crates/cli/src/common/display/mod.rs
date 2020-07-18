pub mod user_actions;

use crate::common::action::ActionResult;
use crate::common::screen::Screen;

/// Shows the specified screen. Since screens (at least as of right now) have
/// no other thing that just an identifier and an associated action, this simply
/// calls the show function in the user_actions module to handle the inner
/// action of the screen.
pub fn show(screen: Screen) -> ActionResult {
    user_actions::show(screen.action)
}

/// Prints a new line to separate content.
pub fn show_line_break() {
    println!();
}

/// Shows a predefined exit message.
pub fn show_exit_message() {
    println!("Bye!");
}
