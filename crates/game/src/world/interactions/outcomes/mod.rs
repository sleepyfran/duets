mod song;

pub use song::*;

use common::entities::SkillWithLevel;

use super::*;

/// Defines the different types of outcomes of an interaction.
#[derive(Clone)]
pub enum Outcome {
    Song(SongOutcome),
    SkillLevelModified(SkillWithLevel, EffectType),
}

pub type InteractionOutcome = Vec<Outcome>;
