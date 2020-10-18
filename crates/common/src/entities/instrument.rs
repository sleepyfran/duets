use serde::{Deserialize, Serialize};

use super::Skill;

/// Defines an instrument that the user can play. If allows_another_instrument is true then it means
/// that it can be combined with another instrument, for example vocals + guitar.
#[derive(Clone, Deserialize, Serialize, Default)]
#[serde(rename_all = "camelCase")]
pub struct Instrument {
    pub name: String,
    pub allows_another_instrument: bool,
    pub associated_skill: Skill,
}
