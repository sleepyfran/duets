use common::entities::{ActionRegistry, ActionTrack, Calendar};

pub trait ActionRegistryCommands {
    /// Updates or inserts the given tracking action into the registry and returns a new reference
    /// to the registry with the updated value.
    fn register(&self, key: &str, calendar: &Calendar) -> ActionRegistry;
}

impl ActionRegistryCommands for ActionRegistry {
    fn register(&self, key: &str, calendar: &Calendar) -> ActionRegistry {
        let mut mutable = self.clone();
        let action_track = ActionTrack {
            time: calendar.time.clone(),
            date: calendar.date,
        };
        let updated_bucket = if let Some(bucket) = mutable.registry.remove(key) {
            bucket.into_iter().chain(vec![action_track]).collect()
        } else {
            vec![action_track]
        };

        mutable.registry.insert(key.into(), updated_bucket);
        mutable
    }
}
