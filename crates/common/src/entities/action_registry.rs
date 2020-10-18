use chrono::NaiveDate;
use serde::{Deserialize, Serialize};
use std::collections::HashMap;

use crate::entities::{Calendar, TimeOfDay};

/// Represents the tracking of one action that was performed with time and date.
#[derive(Clone, Deserialize, Serialize)]
pub struct ActionTrack {
    #[serde(with = "crate::serializables::naivedate::date")]
    pub date: NaiveDate,
    pub time: TimeOfDay,
}

impl Default for ActionTrack {
    fn default() -> ActionTrack {
        ActionTrack {
            date: NaiveDate::from_ymd(2020, 1, 1),
            time: TimeOfDay::Dawn,
        }
    }
}

/// Represents a registry that keeps track of the actions that were performed by the character.
#[derive(Clone, Deserialize, Serialize, Default)]
pub struct ActionRegistry {
    registry: HashMap<String, Vec<ActionTrack>>,
}

impl ActionRegistry {
    /// Updates or inserts the given tracking action into the registry and returns a new reference
    /// to the registry with the updated value.
    pub fn register(&self, key: &str, calendar: &Calendar) -> ActionRegistry {
        let mut mutable = self.clone();
        let updated_bucket = if let Some(bucket) = mutable.registry.remove(key) {
            bucket
                .into_iter()
                .chain(vec![ActionTrack {
                    time: calendar.time.clone(),
                    date: calendar.date,
                }])
                .collect()
        } else {
            vec![]
        };

        mutable.registry.insert(key.into(), updated_bucket);
        mutable
    }

    /// Returns the list of tracking actions of a certain key for a certain date.
    pub fn get_from_date(&self, key: &str, date: NaiveDate) -> Vec<ActionTrack> {
        self.registry
            .get(key)
            .cloned()
            .unwrap_or_else(|| vec![])
            .into_iter()
            .filter(|action| action.date == date)
            .collect()
    }
}
