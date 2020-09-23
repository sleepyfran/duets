use serde::{Deserialize, Serialize};

use super::Identity;
use super::Instrument;

/// Defines the different types of objects that can be in the world. This type will be used to
/// determine the different set of actions that the character can do with the object.
#[derive(Clone, Deserialize, Serialize)]
pub enum ObjectType {
    Instrument(Instrument),
    Computer,
}

/// Defines an object that is positioned in the world and that can be interacted with.
#[derive(Clone, Deserialize, Serialize)]
pub struct Object {
    pub id: String,
    pub name: String,
    pub description: String,
    pub r#type: ObjectType,
}

impl Identity for Object {
    fn id(&self) -> String {
        self.id.clone()
    }
}

impl Default for Object {
    fn default() -> Self {
        Object {
            id: String::default(),
            name: String::default(),
            description: String::default(),
            r#type: ObjectType::Computer,
        }
    }
}
