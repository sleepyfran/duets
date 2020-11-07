use common::entities::{Gender, TimeOfDay};

/// Returns the appropiate clock emoji for the current time of the day.
pub fn for_time<'a>(time: &TimeOfDay) -> &'a str {
    match time {
        TimeOfDay::Dawn => "🕔",
        TimeOfDay::Morning => "🕗",
        TimeOfDay::Midday => "🕛",
        TimeOfDay::Sunset => "🕡",
        TimeOfDay::Dusk => "🕗",
        TimeOfDay::Night => "🕙",
        TimeOfDay::Midnight => "🕛",
    }
}

/// Returns the appropiate emoji for the gender of the character.
pub fn for_gender<'a>(gender: &Gender) -> &'a str {
    match gender {
        Gender::Female => "👩",
        Gender::Male => "🧔",
        Gender::Other => "👤",
    }
}

/// Returns a happy, neutral or sad face depending on the mood of the character.
pub fn for_mood<'a>(mood: u8) -> &'a str {
    match mood {
        0..=35 => "🙁",
        36..=50 => "😐",
        51..=65 => "🙂",
        66..=100 => "😀",
        _ => "🥴",
    }
}

/// Returns the assigned emoji for info.
pub fn for_info<'a>() -> &'a str {
    "✅"
}

/// Returns the assigned emoji for warnings.
pub fn for_warning<'a>() -> &'a str {
    "⚠️"
}

/// Returns the assigned emoji for errors.
pub fn for_error<'a>() -> &'a str {
    "❌"
}

/// Returns the assigned emoji for describing places in the game.
pub fn for_place<'a>() -> &'a str {
    "📍"
}

/// Returns the assigned emoji for interacting with the user.
pub fn for_speech_bubble<'a>() -> &'a str {
    "💬"
}

/// Returns the assigned emoji for showing the character's health.
pub fn for_health<'a>() -> &'a str {
    "❤️"
}

/// Returns the assigned emoji for showing the character's energy.
pub fn for_energy<'a>() -> &'a str {
    "🔋"
}

/// Returns the assigned emoji for showing the character's fame.
pub fn for_fame<'a>() -> &'a str {
    "🌟"
}

/// Returns the assigned emoji for showing the character's skills;
pub fn for_skills<'a>() -> &'a str {
    "🏹"
}

/// Returns the assigned emoji for showing good quality.
pub fn for_good_quality<'a>() -> &'a str {
    "🟢"
}

/// Returns the assigned emoji for showing medium quality.
pub fn for_medium_quality<'a>() -> &'a str {
    "🟡"
}

/// Returns the assigned emoji for showing bad quality.
pub fn for_bad_quality<'a>() -> &'a str {
    "🔴"
}
