use chrono::NaiveDate;

use common::entities::{ActionRegistry, ActionTrack};

pub trait ActionRegistryQueries {
    /// Returns the list of tracking actions of a certain key for a certain date.
    fn get_from_date(&self, key: &str, date: NaiveDate) -> Vec<ActionTrack>;
}

impl ActionRegistryQueries for ActionRegistry {
    fn get_from_date(&self, key: &str, date: NaiveDate) -> Vec<ActionTrack> {
        self.registry
            .get(key)
            .cloned()
            .unwrap_or_else(|| vec![])
            .into_iter()
            .filter(|action| action.date == date)
            .collect()
    }
}
