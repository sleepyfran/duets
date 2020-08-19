use common::entities::Object;

use crate::context::Context;
use crate::shared::display;

/// Attempts to parse an object from all the available ones in the current room. Displays an error
/// if none found and returns the object if it was.
pub fn parse_object_from(args: Vec<String>, global_context: &Context) -> Option<Object> {
    let objects = global_context.get_objects_in_room();
    let object_name = args.join(" ");

    let object = objects.iter().find(|obj| obj.name == object_name);

    match object {
        None => {
            display::show_error(&format!("No object found with the name {}", object_name));
            None
        }
        Some(object) => Some(object.clone()),
    }
}
