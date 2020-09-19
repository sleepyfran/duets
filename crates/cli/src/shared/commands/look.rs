use std::sync::Arc;

use super::Command;
use crate::shared::action::CliAction;
use crate::shared::display;
use crate::shared::lang;
use crate::shared::parsers;

/// Allows the user to get a list of all the objects available in the current room.
pub fn create_look_command() -> Command {
    Command {
        name: String::from("look"),
        matching_names: vec![String::from("l")],
        explanation: String::from("Shows a list of all the objects in the current room"),
        help: r#"
look
----
Shows a list of all the objects in the current room and other rooms that are accesible from here.
Can also be invoked with the following
parameters:

[object name] - Describes the object with the given name. Example: look guitar
[room name] - Describes the room with the given name. Example: look bathroom
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            let rooms = global_context.get_rooms_of_place();
            let objects = global_context.get_objects_in_room();

            if !args.is_empty() {
                let description = parsers::parse_object_from(&args, global_context)
                    .map(|obj| obj.description)
                    .or_else(|| {
                        parsers::parse_room_from(&args, global_context).map(|room| room.description)
                    });

                match description {
                    Some(description) => display::show_text_with_new_line(&description),
                    None => display::show_error(&format!(
                        "No object or room found with the name {}",
                        lang::transformations::join_vec(&args)
                    )),
                }

                return CliAction::Continue;
            }

            if objects.is_empty() {
                display::show_text_with_new_line(&"Seems like there are no objects in here".into());
            } else {
                let objects_description = lang::list::describe_objects(&objects);
                display::show_text_with_new_line(&format!(
                    "You can see in the room {}",
                    objects_description
                ));
            }

            if rooms.is_empty() {
                display::show_text_with_new_line(
                    &"Seems like there are no other rooms in here".into(),
                );
            } else {
                let rooms_description = lang::list::describe_rooms(&rooms);
                display::show_text_with_new_line(&format!(
                    "You can also see {}",
                    rooms_description
                ));
            }

            CliAction::Continue
        }),
    }
}
