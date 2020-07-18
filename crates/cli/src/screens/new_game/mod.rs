use crate::common::action::{ActionResult, Choice, UserAction};
use crate::common::screen::Screen;
use crate::effects;

pub fn create_new_game_screen() -> Screen {
    return Screen {
        name: String::from("New game"),
        action: UserAction::TextInput {
            text: String::from("Creating a new game. What's the name of your character?"),
            on_action: |_input| continue_to_gender_input(),
        },
    };
}

fn continue_to_gender_input() -> ActionResult {
    ActionResult::UserAction(UserAction::TextChoiceInput {
        text: String::from("What's their gender?"),
        choices: vec![
            Choice {
                id: 0,
                text: String::from("male"),
            },
            Choice {
                id: 1,
                text: String::from("female"),
            },
            Choice {
                id: 2,
                text: String::from("other"),
            },
        ],
        on_action: |_choice| continue_to_birthday_input(),
    })
}

fn continue_to_birthday_input() -> ActionResult {
    ActionResult::UserAction(UserAction::DateInput {
        text: String::from("When was its birthday?"),
        on_action: |_birthday| ActionResult::SideEffect(effects::exit),
    })
}
