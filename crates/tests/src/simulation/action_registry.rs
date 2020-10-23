use common::entities::{ActionRegistry, ActionTrack, Calendar, TimeOfDay};
use simulation::commands::action_registry::ActionRegistryCommands;
use simulation::commands::calendar::CalendarCommands;
use simulation::queries::action_registry::ActionRegistryQueries;

fn get_calendar() -> Calendar {
    Calendar::from_year(2020)
}

fn compare_action_track(first: &ActionTrack, second: &ActionTrack) {
    assert_eq!(first.date, second.date);
    assert_eq!(first.time, second.time);
}

fn compare_action_tracks(first: &[ActionTrack], second: &[ActionTrack]) {
    assert_eq!(first.len(), second.len());
    for (index, action_track) in first.iter().enumerate() {
        compare_action_track(action_track, &second[index])
    }
}

/* COMMANDS. */
#[test]
fn register_should_insert_new_record_if_none_existent() {
    let action_registry = ActionRegistry::default();
    let action_registry = action_registry.register("test".into(), &get_calendar());

    assert_eq!(action_registry.registry.len(), 1);
    compare_action_tracks(
        action_registry.registry.get("test").unwrap(),
        &vec![ActionTrack {
            date: get_calendar().date,
            time: get_calendar().time,
        }],
    )
}

#[test]
fn register_should_append_new_record_if_existent() {
    let action_registry = ActionRegistry::default();
    let action_registry = action_registry.register("test".into(), &get_calendar());
    let next_time_calendar = get_calendar().increase_time_once();
    let action_registry = action_registry.register("test".into(), &next_time_calendar);

    assert_eq!(action_registry.registry.len(), 1);
    compare_action_tracks(
        action_registry.registry.get("test").unwrap(),
        &vec![
            ActionTrack {
                date: get_calendar().date,
                time: get_calendar().time,
            },
            ActionTrack {
                date: next_time_calendar.date,
                time: next_time_calendar.time,
            },
        ],
    )
}

/* QUERIES. */
#[test]
fn get_from_date_should_return_empty_if_key_not_found() {
    let action_registry = ActionRegistry::default();
    let action_tracks = action_registry.get_from_date("test".into(), get_calendar().date);
    assert_eq!(action_tracks.len(), 0);
}

#[test]
fn get_from_date_should_return_empty_array_if_key_found_but_not_for_specified_date() {
    let action_registry = ActionRegistry::default();
    let action_registry = action_registry.register("test".into(), &get_calendar());

    let next_date = get_calendar().increase_days(1).date;
    let action_tracks = action_registry.get_from_date("test".into(), next_date);
    assert_eq!(action_tracks.len(), 0);
}

#[test]
fn get_from_date_should_return_array_with_content_from_specified_date() {
    let action_registry = ActionRegistry::default();
    let action_registry = action_registry.register("test".into(), &get_calendar());

    let action_tracks = action_registry.get_from_date("test".into(), get_calendar().date);
    assert_eq!(action_tracks.len(), 1);
}
