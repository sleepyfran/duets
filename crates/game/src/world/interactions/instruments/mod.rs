mod play;

use common::entities::Instrument;

use super::Interaction;

/// Returns all the available interactions for instruments.
pub fn get_interactions(instrument: &Instrument) -> Vec<impl Interaction> {
    vec![play::PlayInteraction {
        instrument: instrument.clone(),
    }]
}
