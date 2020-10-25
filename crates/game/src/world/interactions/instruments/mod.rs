mod compose;
mod play;

use common::entities::Instrument;

use super::Interaction;

/// Returns all the available interactions for instruments.
pub fn get_interactions(instrument: &Instrument) -> Vec<Box<dyn Interaction>> {
    vec![
        Box::new(play::PlayInteraction {
            instrument: instrument.clone(),
        }),
        Box::new(compose::ComposeInteraction {
            instrument: instrument.clone(),
        }),
    ]
}
