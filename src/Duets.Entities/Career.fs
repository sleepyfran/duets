module Duets.Entities.Career

/// Returns the shift duration of the given job.
let jobDuration job =
    match job.Schedule with
    | JobSchedule.Free duration -> duration

/// Retrieves the list of skills required for the given job.
let jobSkills job =
    job.CurrentStage.Requirements
    |> List.map (function
        | CareerStageRequirement.Skill(skillId, _) -> skillId)
