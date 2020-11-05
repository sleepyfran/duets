pub mod compose;
pub mod play;

use std::rc::Rc;

use common::entities::Instrument;

use super::Interaction;

/// Returns all the available interactions for instruments.
pub fn get_interactions(instrument: &Instrument) -> Vec<Rc<dyn Interaction>> {
    vec![
        Rc::new(play::PlayInteraction {
            instrument: instrument.clone(),
        }),
        Rc::new(compose::ComposeInteraction {
            instrument: instrument.clone(),
        }),
    ]
}
