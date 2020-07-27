use chrono::NaiveDate;

/// Allowed times in the day. Instead of implementing a full 24-hour clock, we'll just have a
/// different list of allowed times. Since there are plenty it should be enough to make the game
/// feel quicker than with a 24-hour clock but also slower than just having an action per day.
#[derive(Clone)]
pub enum TimeOfDay {
    Dawn,
    Morning,
    Midday,
    Sunset,
    Dusk,
    Night,
    Midnight,
}

/// Defines the calendar of the game in a specific point of time.
#[derive(Clone)]
pub struct Calendar {
    pub date: NaiveDate,
    pub time: TimeOfDay,
}

impl Calendar {
    pub fn from_year(year: i16) -> Calendar {
        Calendar {
            date: NaiveDate::parse_from_str(&format!("01-01-{}", year), "%d-%m-%Y").unwrap(),
            time: TimeOfDay::Morning,
        }
    }
}
