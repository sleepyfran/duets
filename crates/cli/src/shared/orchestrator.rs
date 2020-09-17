use crate::shared::action::CliAction;
use crate::shared::context;
use crate::shared::display;
use crate::shared::display::prompts;

/// Recursively calls itself with the result of the given CliAction. This
/// will create an infinite loop until we decide to exit using the exit side
/// effect, effectively showing each screen and actions as they're connected.
pub fn start_with(action: CliAction) {
    let global_context = context::get_global_context();

    let result = match action {
        CliAction::Screen(screen) => display::show(screen, &global_context),
        CliAction::SideEffect(effect) => effect(),
        CliAction::Prompt(user_action) => prompts::show(user_action, &global_context),
        _ => CliAction::NoOp,
    };

    display::show_line_break();

    match result {
        CliAction::Chain(first, second) => {
            continue_with(*first);
            continue_with(*second)
        }
        CliAction::NoOp => {}
        action_result => continue_with(action_result),
    }
}

fn continue_with(action: CliAction) {
    start_with(action)
}
