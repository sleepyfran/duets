mod common;
mod effects;
mod screens;

use common::action::Action;
use common::action::Choice;
use common::screen::Screen;
use screens::main_menu;

fn main() {
    let main_menu_screen = main_menu::create_main_screen();
    print!("{}", main_menu_screen.name);

    match main_menu_screen.action {
        Action::ChoiceInput {
            text,
            choices,
            on_action,
        } => {
            println!("{}", text);
            for choice in choices {
                println!("{}, {}", choice.id, choice.text);
            }

            on_action(
                &Choice {
                    id: 0,
                    text: String::from("Test"),
                },
                &Screen {
                    name: String::from("Nothing"),
                    action: Action::NoOp,
                },
            );
        }
        _ => {
            println!("No match in actions");
            std::process::exit(0);
        }
    }
}
