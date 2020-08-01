use engine::entities::TimeOfDay;

/// Returns the appropiate clock emoji for the current time of the day.
pub fn clock_emoji_for_time(time: &TimeOfDay) -> String {
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
