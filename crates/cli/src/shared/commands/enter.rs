use std::sync::Arc;

use game::world::movement;

use super::Command;
use crate::effects;
use crate::shared::action::CliAction;
use crate::shared::display;
use crate::shared::lang;
use crate::shared::parsers;

/// Allows the user to get a list of all the objects available in the current room.
pub fn create_enter_command() -> Command {
    Command {
        name: String::from("enter"),
        matching_names: vec![String::from("e")],
        explanation: String::from("Allows to navigate different rooms in the current place"),
        help: r#"
look
----
Allows to navigate different rooms in the current place. Can be called with:

[room name] - Navigates to the room with the given name. Example: enter bathroom
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            if args.is_empty() {
                display::show_error(
                    "No room specified. Invoke look to see a list of all available rooms",
                );
                CliAction::Continue
            } else {
                let room = parsers::parse_room_from(&args, &global_context);

                match room {
                    Some(room) => {
                        display::show_info(&format!("You entered the {}", room.name));

                        effects::set_state(
                            movement::go_to_room(room, global_context.clone()).game_state,
                        )
                    }
                    None => {
                        display::show_error(&format!(
                            "No room found with the name {}",
                            lang::transformations::join_vec(&args)
                        ));

                        CliAction::Continue
                    }
                }
            }
        }),
    }
}
