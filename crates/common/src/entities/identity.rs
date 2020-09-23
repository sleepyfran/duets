/// Adds a function to allow the retrieval of the ID assigned to the entity that implements it.
pub trait Identity {
    /// Returns the assigned ID of the entity.
    fn id(&self) -> String;
}
