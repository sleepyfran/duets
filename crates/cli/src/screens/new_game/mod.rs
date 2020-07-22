use app::builders::GameBuilder;
use engine::entities::{Character, City, Country, Gender};

use crate::common::action::{Choice, CliAction, Prompt};
use crate::common::context::Context;
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
        on_action: Box::new(|birthday, context| {
            continue_to_city_input(character.with_birthday(birthday), context)
        }),
    })
}

fn continue_to_city_input(character: Character, context: &Context) -> CliAction {
    let cities_from_countries = |countries: Vec<Country>| -> Vec<City> {
        countries
            .iter()
            .map(|c| c.cities.clone())
            .flatten()
            .collect()
    };

    let available_cities = cities_from_countries(context.database.countries.clone());

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
        on_action: Box::new(move |choice, context| {
            let cities = cities_from_countries(context.database.countries.clone());

            let game_builder = GameBuilder::new()
                .with_character(character)
                .with_starting_city(cities[choice.id].to_owned());

            CliAction::Prompt(Prompt::NoOp)
        }),
    })
}
