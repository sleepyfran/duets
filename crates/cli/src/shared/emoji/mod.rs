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
        Gender::Female => "👩".into(),
        Gender::Male => "🧔".into(),
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

/// Returns the assigned emoji for info.
pub fn for_info() -> String {
    "✅".into()
}

/// Returns the assigned emoji for warnings.
pub fn for_warning() -> String {
    "⚠️".into()
}

/// Returns the assigned emoji for errors.
pub fn for_error() -> String {
    "❌".into()
}

/// Returns the assigned emoji for describing places in the game.
pub fn for_place() -> String {
    "📍".into()
}

/// Returns the assigned emoji for interacting with the user.
pub fn for_speech_bubble() -> String {
    "💬".into()
}

/// Returns the assigned emoji for showing the character's health.
pub fn for_health() -> String {
    "❤️".into()
}

/// Returns the assigned emoji for showing the character's fame.
pub fn for_fame() -> String {
    "🌟".into()
}
