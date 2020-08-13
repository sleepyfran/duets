use std::sync::Arc;

use super::Command;
use crate::shared::action::CliAction;
use crate::shared::display;
use crate::shared::emoji;

/// Allows the user to interact with its character and get info about it
pub fn create_character_command() -> Command {
    Command {
        name: String::from("character"),
        matching_names: vec![String::from("c")],
        explanation: String::from("Shows information about the character"),
        help: r#"
character
----
Shows the current status of the character.
        "#
        .into(),
        execute: Arc::new(move |_args, global_context| {
            let character = &global_context.game_state.character;

            display::show_prompt_text_no_emoji(&format!(
                "{} {}",
                emoji::for_gender(&character.gender),
                character.name,
            ));

            display::show_line_break();

            display::show_prompt_text_no_emoji(&format!(
                "{} | {} | {}",
                format!("{} {}", emoji::for_mood(character.mood), character.mood),
                format!("{} {}", emoji::for_health(), character.health),
                format!("{} {}", emoji::for_fame(), character.fame),
            ));

            CliAction::Continue
        }),
    }
}
