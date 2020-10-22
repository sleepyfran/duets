use chrono::{Duration, NaiveDate};

use common::entities::{Calendar, TimeOfDay};

/// Adds a set of functions to increase and decrease skills.
pub trait CalendarCommands {
    /// Increases the number of days keeping the time.
    fn increase_days(self, days: u8) -> Calendar;
    /// Increases the time by a given unit.
    fn increase_time(self, times: u8) -> Calendar;
    /// Increases the time by one unit.
    fn increase_time_once(self) -> Calendar;
}

impl CalendarCommands for Calendar {
    fn increase_days(self, days: u8) -> Calendar {
        Calendar {
            date: self.date + Duration::days(days.into()),
            ..self
        }
    }

    fn increase_time(self, times: u8) -> Calendar {
        (0..times).fold(self, |cal, _| cal.increase_time_once())
    }

    fn increase_time_once(self) -> Calendar {
        if self.time == TimeOfDay::Midnight {
            return Calendar {
                date: self.date + Duration::days(1),
                time: TimeOfDay::Dawn,
            };
        }

        let updated_time = match &self.time {
            TimeOfDay::Dawn => TimeOfDay::Morning,
            TimeOfDay::Morning => TimeOfDay::Midday,
            TimeOfDay::Midday => TimeOfDay::Sunset,
            TimeOfDay::Sunset => TimeOfDay::Dusk,
            TimeOfDay::Dusk => TimeOfDay::Night,
            TimeOfDay::Night => TimeOfDay::Midnight,
            TimeOfDay::Midnight => unreachable!(),
        };

        Calendar {
            time: updated_time,
            ..self
        }
    }
}
