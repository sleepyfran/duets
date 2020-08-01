use chrono::NaiveDate;
use serde::{Deserialize, Serialize};

use engine::entities::{Calendar, TimeOfDay};

/// Allowed times in the day. Instead of implementing a full 24-hour clock, we'll just have a
/// different list of allowed times. Since there are plenty it should be enough to make the game
/// feel quicker than with a 24-hour clock but also slower than just having an action per day.
#[derive(Deserialize, Serialize)]
#[serde(remote = "TimeOfDay")]
pub enum TimeOfDayDef {
    Dawn,
    Morning,
    Midday,
    Sunset,
    Dusk,
    Night,
    Midnight,
}

/// Defines the calendar of the game in a specific point of time.
#[derive(Deserialize, Serialize)]
#[serde(remote = "Calendar")]
pub struct CalendarDef {
    #[serde(with = "super::naivedate::date")]
    pub date: NaiveDate,
    #[serde(with = "TimeOfDayDef")]
    pub time: TimeOfDay,
}
