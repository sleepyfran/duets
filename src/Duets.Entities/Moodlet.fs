module Duets.Entities.Moodlet

/// Creates a moodlet with the given type, start date, and expiration.
let create t startDate expiration =
    { MoodletType = t
      StartedOn = startDate
      Expiration = expiration }
