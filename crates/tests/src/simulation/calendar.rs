use chrono::Duration;

use common::entities::{Calendar, TimeOfDay};
use simulation::commands::calendar::CalendarCommands;

fn get_calendar() -> Calendar {
    Calendar::from_year(2020)
}

fn get_calendar_with_time(time: TimeOfDay) -> Calendar {
    Calendar {
        time,
        ..get_calendar()
    }
}

fn compare_calendars(first: Calendar, second: Calendar) {
    assert_eq!(first.date, second.date);
    assert_eq!(first.time, second.time);
}

/* COMMANDS. */
/* increase_days */
#[test]
fn increase_days_by_0_should_return_same_calendar() {
    let same_day_calendar = get_calendar().increase_days(0);

    compare_calendars(get_calendar(), same_day_calendar);
}

#[test]
fn increase_days_by_1_should_return_calendar_with_next_day() {
    let next_day_calendar = get_calendar().increase_days(1);

    compare_calendars(
        next_day_calendar,
        Calendar {
            date: get_calendar().date + Duration::days(1),
            time: get_calendar().time,
        },
    );
}

#[test]
fn increase_days_by_n_should_return_calendar_with_next_n_days() {
    for n in (10..120) {
        let next_n_calendar = get_calendar().increase_days(n);

        compare_calendars(
            next_n_calendar,
            Calendar {
                date: get_calendar().date + Duration::days(n.into()),
                time: get_calendar().time,
            },
        )
    }
}

/* increase_time_once */
#[test]
fn increase_time_once_of_dawn_should_return_morning() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Dawn).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Morning),
    );
}

#[test]
fn increase_time_once_of_morning_should_return_midday() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Morning).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Midday),
    );
}

#[test]
fn increase_time_once_of_midday_should_return_sunset() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Midday).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Sunset),
    );
}

#[test]
fn increase_time_once_of_sunset_should_return_dusk() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Sunset).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Dusk),
    );
}

#[test]
fn increase_time_once_of_dusk_should_return_night() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Dusk).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Night),
    );
}

#[test]
fn increase_time_once_of_night_should_return_midnight() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Night).increase_time_once(),
        get_calendar_with_time(TimeOfDay::Midnight),
    );
}

#[test]
fn increase_time_once_of_midnight_should_return_next_day_dawn() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Midnight).increase_time_once(),
        Calendar {
            date: get_calendar().date + Duration::days(1),
            time: TimeOfDay::Dawn,
        },
    );
}

/* increase_time */
#[test]
fn increase_time_by_0_should_return_same_calendar() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Morning).increase_time(0),
        get_calendar_with_time(TimeOfDay::Morning),
    );
}

#[test]
fn increase_time_by_1_should_be_the_same_as_increase_time_once() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Morning).increase_time(1),
        get_calendar_with_time(TimeOfDay::Morning).increase_time_once(),
    );
}

#[test]
fn increase_time_by_2_should_be_the_same_as_increase_time_once_two_times() {
    compare_calendars(
        get_calendar_with_time(TimeOfDay::Morning).increase_time(2),
        get_calendar_with_time(TimeOfDay::Morning)
            .increase_time_once()
            .increase_time_once(),
    );
}
