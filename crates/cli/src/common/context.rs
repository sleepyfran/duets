use app::database::Database;

use crate::common::action::CliAction;

/// Context passed to every action performed in the CLI. Contains any stateful values that might
/// be necessary during execution.
#[derive(Clone)]
pub struct Context {
    pub database: Database,
}

/// Context that each step will hold to have quick access to the global context and other goodies
/// that we might need. Also defines the next_action to execute so we can programatically come back
/// to a certain step in case of failure.
pub struct ScreenContext<Builder> {
    pub global_context: Context,
    pub game_builder: Builder,
    pub next_action: Option<Box<dyn FnOnce(ScreenContext<Builder>) -> CliAction>>,
}
