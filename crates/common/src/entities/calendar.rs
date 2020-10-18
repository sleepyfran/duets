use chrono::{Duration, NaiveDate};
use serde::{Deserialize, Serialize};
use std::fmt::{Debug, Display, Formatter, Result};

/// Allowed times in the day. Instead of implementing a full 24-hour clock, we'll just have a
/// different list of allowed times. Since there are plenty it should be enough to make the game
/// feel quicker than with a 24-hour clock but also slower than just having an action per day.
#[derive(Clone, Debug, Deserialize, Serialize, PartialEq)]
pub enum TimeOfDay {
    Dawn,
    Morning,
    Midday,
    Sunset,
    Dusk,
    Night,
    Midnight,
}

impl Display for TimeOfDay {
    fn fmt(&self, f: &mut Formatter) -> Result {
        Debug::fmt(self, f)
    }
}

/// Defines the calendar of the game in a specific point of time.
#[derive(Clone, Deserialize, Serialize)]
pub struct Calendar {
    #[serde(with = "crate::serializables::naivedate::date")]
    pub date: NaiveDate,
    pub time: TimeOfDay,
}

impl Default for Calendar {
    fn default() -> Calendar {
        Calendar {
            date: NaiveDate::from_yo(2010, 1),
            time: TimeOfDay::Morning,
        }
    }
}

impl Calendar {
    /// Creates a calendar from a given year selecting the first year of it.
    pub fn from_year(year: i16) -> Calendar {
        Calendar {
            date: NaiveDate::parse_from_str(&format!("01-01-{}", year), "%d-%m-%Y").unwrap(),
            time: TimeOfDay::Morning,
        }
    }

    /// Increases the time by one unit.
    pub fn increase_time_once(self) -> Calendar {
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
