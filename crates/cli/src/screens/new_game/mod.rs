use chrono::Datelike;

use app::builders::{GameStartBuilder, ValidationError};
use engine::entities::{City, Country, Gender};

use crate::common::action::{Choice, CliAction, DateFormat, Prompt};
use crate::common::context::ScreenContext;
use crate::common::display;
use crate::common::screen::Screen;

type NewGameContext = ScreenContext<GameStartBuilder>;

/// Creates a new game screen that handles the creation of the character as well as the first
/// band of the character.
pub fn create_new_game_screen() -> Screen {
    Screen {
        name: String::from("New game"),
        action: Prompt::TextInput {
            text: String::from("Creating a new game. What's the name of your character?"),
            on_action: Box::new(|input, global_context| {
                continue_to_gender_input(NewGameContext {
                    global_context: global_context.clone(),
                    game_builder: GameStartBuilder::default().name(input),
                    next_action: Some(Box::new(continue_to_birthday_input)),
                })
            }),
        },
    }
}

fn continue_to_gender_input(context: NewGameContext) -> CliAction {
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
        on_action: Box::new(|choice, global_context| {
            context.next_action.unwrap()(NewGameContext {
                global_context: global_context.clone(),
                game_builder: context.game_builder.gender(match choice.id {
                    0 => Gender::Male,
                    1 => Gender::Female,
                    _ => Gender::Other,
                }),
                next_action: Some(Box::new(continue_to_city_input)),
            })
        }),
    })
}

fn continue_to_birthday_input(context: NewGameContext) -> CliAction {
    CliAction::Prompt(Prompt::DateInput {
        text: String::from("When was its birthday?"),
        format: DateFormat::Full,
        on_action: Box::new(|birthday, global_context| {
            context.next_action.unwrap()(NewGameContext {
                global_context: global_context.clone(),
                game_builder: context.game_builder.birthday(birthday),
                next_action: Some(Box::new(continue_to_start_year_input)),
            })
        }),
    })
}

fn continue_to_city_input(context: NewGameContext) -> CliAction {
    let cities_from_countries = |countries: Vec<Country>| -> Vec<City> {
        countries
            .iter()
            .map(|c| c.cities.clone())
            .flatten()
            .collect()
    };

    let available_cities = cities_from_countries(context.global_context.database.countries.clone());

    CliAction::Prompt(Prompt::ChoiceInput {
        text: String::from("Where are they from?"),
        choices: available_cities
            .iter()
            .enumerate()
            .map(|(index, city)| Choice {
                id: index,
                text: format!("{}, {}", city.name, city.country_name),
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let cities = cities_from_countries(global_context.database.countries.clone());

            context.next_action.unwrap()(NewGameContext {
                global_context: global_context.clone(),
                game_builder: context
                    .game_builder
                    .start_city(cities[choice.id].to_owned()),
                next_action: Some(Box::new(continue_to_validation)),
            })
        }),
    })
}

fn continue_to_start_year_input(context: NewGameContext) -> CliAction {
    CliAction::Prompt(Prompt::DateInput {
        text: String::from("What year should the game start from?"),
        format: DateFormat::Year,
        on_action: Box::new(move |date, global_context| {
            context.next_action.unwrap()(NewGameContext {
                global_context: global_context.clone(),
                game_builder: context.game_builder.start_year(date.year() as i16),
                next_action: Some(Box::new(continue_to_confirmation)),
            })
        }),
    })
}

fn continue_to_validation(context: NewGameContext) -> CliAction {
    match context.game_builder.clone().validate() {
        Err(ValidationError::BirthdayTooEarly) => {
            display::show_error(&String::from(
                "The character's birthday should be after 1950",
            ));
            continue_to_birthday_input(NewGameContext {
                global_context: context.global_context.clone(),
                game_builder: context.game_builder.clone(),
                next_action: Some(Box::new(continue_to_validation)),
            })
        }
        Err(ValidationError::CharacterNot18WhenGameStarts) => {
            display::show_error(&String::from(
                "The character has to be 18 by the time the game starts",
            ));
            continue_to_start_year_input(NewGameContext {
                global_context: context.global_context.clone(),
                game_builder: context.game_builder.clone(),
                next_action: Some(Box::new(continue_to_validation)),
            })
        }
        Err(ValidationError::InvalidName) => {
            display::show_error(&String::from("The given name is not valid"));
            CliAction::Prompt(Prompt::NoOp)
        }
        Ok(()) => context.next_action.unwrap()(NewGameContext {
            global_context: context.global_context.clone(),
            game_builder: context.game_builder,
            next_action: None,
        }),
    }
}

fn continue_to_confirmation(context: NewGameContext) -> CliAction {
    let game_state = context.game_builder.build().unwrap().to_game_state();

    display::show_line_break();
    display::show_text_with_new_line(&String::from("We have everything!"));
    display::show_text(&format!(
        "This will create a character named {} who was born in {} and lives in {}",
        game_state.character.name, game_state.character.birthday, game_state.current_city.name,
    ));

    CliAction::Prompt(Prompt::ChoiceInput {
        text: String::from(""),
        choices: vec![
            Choice {
                id: 0,
                text: String::from("Yes, looks good"),
            },
            Choice {
                id: 1,
                text: String::from("No, let me start again"),
            },
        ],
        on_action: Box::new(|choice, _global_context| match choice.id {
            0 => {
                display::show_text(&String::from("Awesome!"));
                CliAction::Prompt(Prompt::NoOp)
            }
            1 => {
                display::show_text(&String::from("Let's get to it"));
                CliAction::Prompt(Prompt::NoOp)
            }
            _ => CliAction::Prompt(Prompt::NoOp),
        }),
    })
}
