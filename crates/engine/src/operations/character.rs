use common::entities::Character;

pub trait CharacterOperations {
    /// Checks that the health of the given character is at least the given min_health.
    fn health_above(&self, min: i8) -> bool;
    /// Checks that the mood of the given self is at least given min_mood.
    fn mood_above(&self, min: i8) -> bool;
}

impl CharacterOperations for Character {
    fn health_above(&self, min_health: i8) -> bool {
        self.health > min_health
    }

    fn mood_above(&self, min_mood: i8) -> bool {
        self.mood > min_mood
    }
}
