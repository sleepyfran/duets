use std::sync::Arc;

use app::world::movement;
use app::world::places::Places;

use super::Command;
use crate::effects;
use crate::shared::action::{Choice, CliAction, ConfirmationChoice, Prompt, PromptText};
use crate::shared::context::Context;

/// Allows the user to get info about the character's position and go to other places.
pub fn create_map_command() -> Command {
    Command {
        name: String::from("map"),
        matching_names: vec![String::from("m")],
        explanation: String::from("Shows information about the current position of the character as well and allows to travel to another place in the city"),
        help: r#"
map
----
Shows the current position of the character and displays a selector for quick travelling
to another place in the city.
        "#
        .into(),
        execute: Arc::new(move |_args, global_context| {
            let character_name = &global_context.game_state.character.name;
            let position = &global_context.game_state.position;

            CliAction::Prompt(Prompt::ConfirmationInput {
                text: PromptText::WithEmoji(format!(
                    "{} is currently in {}, in the city of {}. Do you want to go to another place?",
                    character_name,
                    position.place.name,
                    position.city.name,
                )),
                on_action: Box::new(|choice, inner_global_context| {
                    match choice {
                        ConfirmationChoice::Yes => show_place_choice(inner_global_context),
                        ConfirmationChoice::No => CliAction::Continue,
                    }
                }),
            })
        }),
    }
}

fn show_place_choice(global_context: &Context) -> CliAction {
    let places = global_context.get_places_of_city();

    CliAction::Prompt(Prompt::ChoiceInput {
        text: PromptText::WithEmoji(String::from("Where do you want to go?")),
        choices: places
            .iter()
            .enumerate()
            .map(|(index, place)| Choice {
                id: index,
                text: place.name.clone(),
            })
            .collect(),
        on_action: Box::new(move |choice, global_context| {
            let selected_place = places[choice.id].clone();

            effects::set_state(
                movement::go_to_place(selected_place, global_context.clone()).game_state,
            )
        }),
    })
}
