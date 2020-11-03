use std::sync::Mutex;

pub use game::context::Context;

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
pub struct ScreenContext<State> {
    pub global_context: Context,
    pub state: State,
    pub next_action: NextAction<State>,
}

pub type NextAction<State> = Option<Box<dyn FnOnce(ScreenContext<State>) -> CliAction>>;

impl<S> ScreenContext<S> {
    /// Overrides the current context and returns a copy of the ScreenContext with it.
    pub fn with_context(self, global_context: &Context) -> Self {
        Self {
            global_context: global_context.clone(),
            ..self
        }
    }

    /// Overrides the current next_action and returns a copy of the ScreenContext with it.
    pub fn with_next_action(self, next_action: NextAction<S>) -> Self {
        Self {
            next_action,
            ..self
        }
    }

    /// Overrides the current state returning a copy of the context with the new state.
    pub fn modify_state<F>(self, modify_fn: F) -> Self
    where
        F: FnOnce(S) -> S,
    {
        Self {
            state: modify_fn(self.state),
            ..self
        }
    }
}
