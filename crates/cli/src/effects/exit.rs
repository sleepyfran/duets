use crate::common::action::CliAction;

/// Exits completely from the CLI.
pub fn exit() -> Option<CliAction> {
    std::process::exit(0);
}
