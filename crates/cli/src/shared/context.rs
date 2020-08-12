use std::sync::Mutex;

pub use app::context::Context;

use crate::shared::action::CliAction;

lazy_static! {
    pub static ref CONTEXT_CONTAINER: Mutex<Context> = Mutex::new(Context::default());
}

/// Retrieves a clone of the current global context.
pub fn get_global_context() -> Context {
    CONTEXT_CONTAINER.lock().unwrap().clone()
}

/// Sets the current global context.
pub fn set_global_context(context: Context) {
    *CONTEXT_CONTAINER.lock().unwrap() = context
}

/// Modifies the content from the global context by passing the current one to a function that will
/// return a modified one.
pub fn modify_global_context(modify_fn: Box<dyn FnOnce(Context) -> Context>) {
    set_global_context(modify_fn(get_global_context()))
}

/// Context that each step will hold to have quick access to the global context and other goodies
/// that we might need. Also defines the next_action to execute so we can programatically come back
/// to a certain step in case of failure.
pub struct ScreenContext<Builder> {
    pub global_context: Context,
    pub game_builder: Builder,
    pub next_action: Option<Box<dyn FnOnce(ScreenContext<Builder>) -> CliAction>>,
}
