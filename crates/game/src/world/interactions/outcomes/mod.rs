mod song;

pub use song::*;

use super::*;

/// Defines the different types of outcomes of an interaction.
#[derive(Clone)]
pub enum Outcome {
    Song(SongOutcome),
}

pub type InteractionOutcome = Vec<Outcome>;
