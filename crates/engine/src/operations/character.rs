use common::entities::Character;

/// Checks that the health of the given character is at least the given min_health.
pub fn health_above(character: &Character, min_health: i8) -> bool {
    character.health > min_health
}

/// Checks that the mood of the given character is at least given min_mood.
pub fn mood_above(character: &Character, min_mood: i8) -> bool {
    character.mood > min_mood
}
