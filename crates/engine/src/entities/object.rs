use super::Instrument;

/// Defines the different types of objects that can be in the world. This type will be used to
/// determine the different set of actions that the character can do with the object.
#[derive(Clone)]
pub enum ObjectType {
    Instrument(Instrument),
    Computer,
}

/// Defines an object that is positioned in the world and that can be interacted with.
#[derive(Clone)]
pub struct Object {
    pub id: String,
    pub name: String,
    pub description: String,
    pub r#type: ObjectType,
}
