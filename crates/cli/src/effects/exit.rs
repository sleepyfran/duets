use crate::common::action::ActionResult;

/// Exits completely from the CLI.
pub fn exit() -> Option<ActionResult> {
    std::process::exit(0);
}
