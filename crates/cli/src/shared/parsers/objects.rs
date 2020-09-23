use app::world::World;
use common::entities::Object;

use crate::context::Context;
use crate::shared::lang;

/// Attempts to parse an object from all the available ones in the current room.
pub fn parse_object_from(args: &Vec<String>, global_context: &Context) -> Option<Object> {
    let objects = global_context.get_objects_in_room();
    let object_name = lang::transformations::join_vec(args);

    objects.iter().find(|obj| obj.name == object_name).cloned()
}
