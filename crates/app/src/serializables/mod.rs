/// Same entities as defined in the engine but with support for being serialized and deserialized
/// to JSON. This ensures that the engine has no external dependencies on whatever format we decide
/// to use. Each of these entities expose a `to_engine` and `from_engine` functions to transform
/// the entity to the engine representation or to the app representation.
mod character;
mod city;
mod country;
mod game_state;
mod naivedate;
mod skill;

pub use character::CharacterDef;
pub use city::CityDef;
pub use country::CountryDef;
pub use game_state::GameState;
pub use skill::{SkillCategoryDef, SkillDef, SkillWithLevelDef};
