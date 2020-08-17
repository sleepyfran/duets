use in_definite;
use std::sync::Arc;

use super::Command;
use crate::shared::action::CliAction;
use crate::shared::display;
use crate::shared::lang;

/// Allows the user to get a list of all the objects available in the current room.
pub fn create_look_command() -> Command {
    Command {
        name: String::from("look"),
        matching_names: vec![String::from("l")],
        explanation: String::from("Shows a list of all the objects in the current room"),
        help: r#"
look
----
Shows a list of all the objects in the current room. Can also be invoked with the following
parameters:

[object name] - Describes the object with the given name. Example: look guitar
        "#
        .into(),
        execute: Arc::new(move |args, global_context| {
            let objects = global_context.get_objects_in_room();

            if !args.is_empty() {
                let object_name = args.join(" ");

                let object = objects.iter().find(|obj| obj.name == object_name);

                match object {
                    None => display::show_error(&format!(
                        "No element found with the name {}",
                        object_name
                    )),
                    Some(object) => display::show_text_with_new_line(&object.description),
                }

                return CliAction::Continue;
            }

            if objects.is_empty() {
                display::show_text_with_new_line(&"Seems like there are no objects in here".into());
            } else {
                let list_description = lang::list::describe(&objects);
                display::show_text_with_new_line(&format!(
                    "You can see in the room {}",
                    list_description
                ));
            }

            CliAction::Continue
        }),
    }
}
