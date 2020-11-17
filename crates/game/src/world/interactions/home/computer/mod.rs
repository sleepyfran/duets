pub mod browse_web;

use std::rc::Rc;

use super::super::Interaction;

/// Returns all the available interactions for a computer.
pub fn get_interactions() -> Vec<Rc<dyn Interaction>> {
    vec![Rc::new(browse_web::BrowseWebInteraction)]
}
