module Duets.Entities.Career

/// Returns the shift duration of the given job.
let jobDuration job =
    match job.CurrentStage.Schedule with
    | JobSchedule.Free duration -> duration
    | JobSchedule.Fixed (_, duration) -> duration

/// Retrieves the list of skills required for the given job.
let jobSkills job =
    job.CurrentStage.Requirements
    |> List.choose (function
        | CareerStageRequirement.Skill(skillId, _) -> Some skillId
        | _ -> None)
