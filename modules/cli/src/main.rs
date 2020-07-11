mod common;
mod effects;

use common::action::Action;

fn main() {
    println!("Hello, world!");
    let action = Action::TextInput {
        on_action: |input, _screen| {
            print!("{}", input);
            return effects::exit();
        },
    };

    match action {
        Action::TextInput { on_action } => drop(on_action(
            "test".to_string(),
            common::screen::Screen {
                name: "test".to_string(),
                action: Action::NoOp,
            },
        )),
        _ => print!("Welcome to the new age! :O"),
    }
}
