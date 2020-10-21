use common::entities::Character;

/// Set of functions to query values related to the current character status.
pub trait Status {
    /// Checks that the energy of the given character is at least the given min_energy.
    fn energy_above(&self, min_energy: u8) -> bool;
    /// Checks that the health of the given character is at least the given min_health.
    fn health_above(&self, min: u8) -> bool;
    /// Checks that the mood of the given character is at least the given min_mood.
    fn mood_above(&self, min: u8) -> bool;
}

impl Status for Character {
    fn energy_above(&self, min_energy: u8) -> bool {
        self.energy > min_energy
    }

    fn health_above(&self, min_health: u8) -> bool {
        self.health > min_health
    }

    fn mood_above(&self, min_mood: u8) -> bool {
        self.mood > min_mood
    }
}
