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
        execute: Arc::new(move |_args, global_context| {
            let character = &global_context.game_state.character;

            display::show_line_break();
            display::show_prompt_text_no_emoji(&format!(
                "{} {}",
                emoji::for_gender(&character.gender),
                character.name,
            ));

            display::show_prompt_text_no_emoji(&format!(
                "\n{} | {} | {}\n",
                format!("{} {}", emoji::for_mood(character.mood), character.mood),
                format!("{}  {}", "♥️", character.health),
                format!("{} {}", "⭐️", character.fame),
            ));

            CliAction::Continue
        }),
    }
}
