module Duets.Entities.Moodlet

/// Creates a moodlet with the given type, start date, and expiration.
let create t startDate expiration =
    { MoodletType = t
      StartedOn = startDate
      Expiration = expiration }

/// Returns the days since the moodlet started.
let daysSinceStart moodlet currentDate =
    Calendar.Query.daysBetween moodlet.StartedOn currentDate

/// Returns the day moments since the moodlet started.
let dayMomentsSinceStart moodlet currentDate =
    Calendar.Query.dayMomentsBetween moodlet.StartedOn currentDate
