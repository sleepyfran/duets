use chrono::NaiveDate;
use serde::{Deserialize, Serialize};
use std::collections::HashMap;

use crate::entities::TimeOfDay;

/// Represents the tracking of one action that was performed with time and date.
#[derive(Clone, Deserialize, Serialize, Debug)]
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
    pub registry: HashMap<String, Vec<ActionTrack>>,
}
