use crate::common::action::Action;
use crate::common::action::Choice;
use crate::common::screen::Screen;
use crate::effects;

pub fn create_main_screen() -> Screen {
    return Screen {
        name: String::from("Main Menu"),
        action: Action::ChoiceInput {
            text: String::from("Welcome to Duets! Select an option to begin"),
            choices: vec![
                Choice {
                    id: 0,
                    text: String::from("Start new game"),
                },
                Choice {
                    id: 1,
                    text: String::from("Load game"),
                },
                Choice {
                    id: 2,
                    text: String::from("Exit"),
                },
            ],
            on_action: |choice, _screen| match choice.id {
                0 => effects::exit(),
                1 => effects::exit(),
                2 => effects::exit(),
                _ => panic!("Invalid input!"),
            },
        },
    };
}
