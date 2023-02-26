module Duets.Entities.Career

/// Returns the shift duration of the given job.
let jobDuration job =
    match job.Schedule with
    | JobSchedule.Free duration -> duration
