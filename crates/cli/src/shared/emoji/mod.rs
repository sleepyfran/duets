use engine::entities::{Gender, TimeOfDay};

/// Returns the appropiate clock emoji for the current time of the day.
pub fn for_time(time: &TimeOfDay) -> String {
    match time {
        TimeOfDay::Dawn => "🕔".into(),
        TimeOfDay::Morning => "🕗".into(),
        TimeOfDay::Midday => "🕛".into(),
        TimeOfDay::Sunset => "🕡".into(),
        TimeOfDay::Dusk => "🕗".into(),
        TimeOfDay::Night => "🕙".into(),
        TimeOfDay::Midnight => "🕛".into(),
    }
}

/// Returns the appropiate emoji for the gender of the character.
pub fn for_gender(gender: &Gender) -> String {
    match gender {
        Gender::Female => "👩🏻‍🦰".into(),
        Gender::Male => "🧔🏻".into(),
        Gender::Other => "👤".into(),
    }
}

/// Returns a happy, neutral or sad face depending on the mood of the character.
pub fn for_mood(mood: i8) -> String {
    match mood {
        0..=35 => "🙁".into(),
        36..=50 => "😐".into(),
        51..=65 => "🙂".into(),
        66..=100 => "😀".into(),
        _ => "🥴".into(),
    }
}
