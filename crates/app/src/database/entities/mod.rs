mod city;
mod country;

/// Same entities as defined in the engine but with support for being serialized and deserialized
/// to JSON. This ensures that the engine has no external dependencies on whatever format we decide
/// to use. Each of these entities expose a `to_engine` and `from_engine` functions to transform
/// the entity to the engine representation or to the app representation.
pub use city::City;
pub use country::Country;
