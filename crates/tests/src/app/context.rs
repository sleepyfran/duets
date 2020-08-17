use app::context::Context;
use app::database::Database;
use common::serializables::{GameState, Position};
use engine::entities::{City, Country, Place, Room};

#[test]
fn validate_integrity_errors_when_game_state_is_default() {
    let invalid_country_context = Context {
        database: Database {
            ..Database::default()
        },
        game_state: GameState {
            ..GameState::default()
        },
    };
    assert_eq!(invalid_country_context.validate_integrity().is_err(), true);
}

#[test]
fn validate_integrity_errors_when_game_state_contains_invalid_country_reference() {
    let invalid_country_context = Context {
        database: Database {
            countries: vec![Country {
                id: "test_country".into(),
                name: "Test".into(),
                cities: vec![City::default()],
                population: 0,
            }],
            ..Database::default()
        },
        game_state: GameState {
            position: Position {
                country: Country {
                    id: "invalid_country".into(),
                    ..Country::default()
                },
                ..Position::default()
            },
            ..GameState::default()
        },
    };
    assert_eq!(invalid_country_context.validate_integrity().is_err(), true);
}

#[test]
fn validate_integrity_errors_when_game_state_contains_invalid_city_reference() {
    let invalid_country_context = Context {
        database: Database {
            countries: vec![Country {
                id: "test_country".into(),
                name: "Test".into(),
                cities: vec![City {
                    id: "test_city".into(),
                    ..City::default()
                }],
                population: 0,
            }],
            ..Database::default()
        },
        game_state: GameState {
            position: Position {
                country: Country {
                    id: "test_country".into(),
                    ..Country::default()
                },
                city: City {
                    id: "invalid_city".into(),
                    ..City::default()
                },
                ..Position::default()
            },
            ..GameState::default()
        },
    };
    assert_eq!(invalid_country_context.validate_integrity().is_err(), true);
}

#[test]
fn validate_integrity_errors_when_game_state_contains_invalid_place_reference() {
    let invalid_country_context = Context {
        database: Database {
            countries: vec![Country {
                id: "test_country".into(),
                name: "Test".into(),
                cities: vec![City {
                    id: "test_city".into(),
                    places: vec![Place {
                        id: "test_place".into(),
                        ..Place::default()
                    }],
                    ..City::default()
                }],
                ..Country::default()
            }],
            ..Database::default()
        },
        game_state: GameState {
            position: Position {
                country: Country {
                    id: "test_country".into(),
                    ..Country::default()
                },
                city: City {
                    id: "test_city".into(),
                    ..City::default()
                },
                place: Place {
                    id: "invalid_place".into(),
                    ..Place::default()
                },
                ..Position::default()
            },
            ..GameState::default()
        },
    };
    assert_eq!(invalid_country_context.validate_integrity().is_err(), true);
}

#[test]
fn validate_integrity_errors_when_game_state_contains_invalid_room_reference() {
    let invalid_country_context = Context {
        database: Database {
            countries: vec![Country {
                id: "test_country".into(),
                name: "Test".into(),
                cities: vec![City {
                    id: "test_city".into(),
                    places: vec![Place {
                        id: "test_place".into(),
                        rooms: vec![Room {
                            id: "test_room".into(),
                            ..Room::default()
                        }],
                        ..Place::default()
                    }],
                    ..City::default()
                }],
                population: 0,
            }],
            ..Database::default()
        },
        game_state: GameState {
            position: Position {
                country: Country {
                    id: "test_country".into(),
                    ..Country::default()
                },
                city: City {
                    id: "test_city".into(),
                    ..City::default()
                },
                place: Place {
                    id: "test_place".into(),
                    ..Place::default()
                },
                room: Room {
                    id: "invalid_room".into(),
                    ..Room::default()
                },
                ..Position::default()
            },
            ..GameState::default()
        },
    };
    assert_eq!(invalid_country_context.validate_integrity().is_err(), true);
}