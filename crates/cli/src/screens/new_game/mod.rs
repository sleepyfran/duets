use engine::entities::{Character, Gender};

use crate::common::action::{Choice, CliAction, Prompt};
use crate::common::screen::Screen;
use crate::effects;

pub fn create_new_game_screen() -> Screen {
    Screen {
        name: String::from("New game"),
        action: Prompt::TextInput {
            text: String::from("Creating a new game. What's the name of your character?"),
            on_action: Box::new(|input, _context| {
                continue_to_gender_input(Character::new(input.to_string()))
            }),
        },
    }
}

fn continue_to_gender_input(character: Character) -> CliAction {
    CliAction::Prompt(Prompt::ChoiceInput {
        text: String::from("What's their gender?"),
        choices: vec![
            Choice {
                id: 0,
                text: String::from("Male"),
            },
            Choice {
                id: 1,
                text: String::from("Female"),
            },
            Choice {
                id: 2,
                text: String::from("Other"),
            },
        ],
        on_action: Box::new(|choice, _context| {
            continue_to_birthday_input(character.with_gender(match choice.id {
                0 => Gender::Male,
                1 => Gender::Female,
                _ => Gender::Other,
            }))
        }),
    })
}

fn continue_to_birthday_input(character: Character) -> CliAction {
    CliAction::Prompt(Prompt::DateInput {
        text: String::from("When was its birthday?"),
        on_action: Box::new(|_birthday, _context| CliAction::SideEffect(effects::exit)),
    })
}
