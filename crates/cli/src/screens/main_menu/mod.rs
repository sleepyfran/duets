use super::new_game;
use crate::common::action::ActionResult;
use crate::common::action::Choice;
use crate::common::action::Prompt;
use crate::common::screen::Screen;
use crate::effects;

pub fn create_main_screen() -> Screen {
    return Screen {
        name: String::from("Main Menu"),
        action: Prompt::ChoiceInput {
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
            on_action: |choice| match choice.id {
                0 => ActionResult::Screen(new_game::create_new_game_screen()),
                1 => ActionResult::SideEffect(effects::exit),
                2 => ActionResult::SideEffect(effects::exit),
                _ => ActionResult::Prompt(Prompt::NoOp),
            },
        },
    };
}
