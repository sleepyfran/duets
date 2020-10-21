use common::entities::Character;
use common::shared::bound_to_positive_hundred;

/// Adds a set of functions to increase and decrease skills.
pub trait CharacterCommands {
    /// Increases health by the specified amount if possible (< 100)
    fn increase_health_by(self, amount: u8) -> Character;
    /// Decreases health by the specified amount if possible (< 100)
    fn decrease_health_by(self, amount: u8) -> Character;
    /// Increases energy by the specified amount if possible (< 100)
    fn increase_energy_by(self, amount: u8) -> Character;
    /// Decreases energy by the specified amount if possible (< 100)
    fn decrease_energy_by(self, amount: u8) -> Character;
}

impl CharacterCommands for Character {
    fn increase_health_by(self, amount: u8) -> Character {
        let health = &self.health.saturating_add(amount);
        self.with_health(*health)
    }
    fn decrease_health_by(self, amount: u8) -> Character {
        let health = &self.health.saturating_sub(amount);
        self.with_health(*health)
    }
    fn increase_energy_by(self, amount: u8) -> Character {
        let energy = &self.energy.saturating_add(amount);
        self.with_energy(*energy)
    }
    fn decrease_energy_by(self, amount: u8) -> Character {
        let energy = &self.energy.saturating_sub(amount);
        self.with_energy(*energy)
    }
}
