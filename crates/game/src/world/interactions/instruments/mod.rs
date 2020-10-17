mod play;

use super::Interaction;

/// Returns all the available interactions for instruments.
pub fn get_interactions() -> Vec<impl Interaction> {
    vec![play::PlayInteraction]
}
