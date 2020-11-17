pub mod sleep;

use std::rc::Rc;

use super::super::Interaction;

/// Returns all the available interactions for a bed.
pub fn get_interactions() -> Vec<Rc<dyn Interaction>> {
    vec![Rc::new(sleep::SleepInteraction)]
}
