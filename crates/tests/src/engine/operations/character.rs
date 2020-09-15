use common::entities::Character;
use engine::operations;

#[test]
fn check_health_should_return_false_if_health_below_min() {
    let character = Character {
        health: 10,
        ..Character::default()
    };

    assert_eq!(operations::character::health_above(&character, 20), false);
}

#[test]
fn check_health_should_return_false_if_health_equals_min() {
    let character = Character {
        health: 20,
        ..Character::default()
    };

    assert_eq!(operations::character::health_above(&character, 20), false);
}

#[test]
fn check_health_should_return_true_if_health_above_min() {
    let character = Character {
        health: 100,
        ..Character::default()
    };

    assert_eq!(operations::character::health_above(&character, 20), true);
}

#[test]
fn check_mood_should_return_false_if_mood_below_min() {
    let character = Character {
        mood: 10,
        ..Character::default()
    };

    assert_eq!(operations::character::mood_above(&character, 20), false);
}

#[test]
fn check_mood_should_return_false_if_mood_equals_min() {
    let character = Character {
        mood: 20,
        ..Character::default()
    };

    assert_eq!(operations::character::mood_above(&character, 20), false);
}

#[test]
fn check_mood_should_return_true_if_mood_above_min() {
    let character = Character {
        mood: 100,
        ..Character::default()
    };

    assert_eq!(operations::character::mood_above(&character, 20), true);
}
