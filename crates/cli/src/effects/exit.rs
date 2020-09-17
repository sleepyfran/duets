use crate::shared::action::CliAction;

/// Exits completely from the CLI.
pub fn exit() -> CliAction {
    std::process::exit(0);
}
