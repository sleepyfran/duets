use crate::common::action::ActionResult;

/// Exits completely from the CLI.
pub fn exit() -> ActionResult {
  return ActionResult::SideEffect(|| std::process::exit(0));
}
